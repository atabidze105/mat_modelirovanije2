using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using mat_modelirovanije2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static mat_modelirovanije2.Helper;


namespace mat_modelirovanije2
{
    public partial class MainWindow : Window
    {
        private List<Employee> _Employees = DBContext.Employees.Include(x => x.Job).ToList();
        private List<Event> _Events = DBContext.Events.Include(x => x.IdOrganisatorNavigation).ToList();
        private List<News> _News = new List<News>();
        private DispatcherTimer _DispatcherTimer = new DispatcherTimer() { Interval = new System.TimeSpan(0,0,15) };
        private List<Employee> _EmployeesFound = new();
        private List<Event> _EventsFound = new();
        private List<News> _NewsFound = new();



        public MainWindow()
        {
            InitializeComponent();
            lbox_employee.ItemsSource = _Employees.ToList();
            lbox_events.ItemsSource = _Events.ToList();
            NewsUpdate();
            _DispatcherTimer.Start();
            _DispatcherTimer.Tick += (s, e) =>
            {
                NewsUpdate();
            };

            calendar_custom.Loaded += OnCalendarLoaded;
            calendar_custom.DisplayDateChanged += CustomCalendar_DisplayDateChanged;
            calendar_custom.SelectedDatesChanged += Calendar_custom_SelectedDatesChanged;
        }


        private void Calendar_custom_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            BrushesCalendar();
        }

        private void NewsUpdate()
        {
            _News.Clear();
            _News = JsonConvert.DeserializeObject<List<News>>(File.ReadAllText("Assets/news_response.json"));

            if (tbox_searchbar.Text == "" || tbox_searchbar.Text == null)
                lbox_news.ItemsSource = _News.ToList();
            else
                lbox_news.ItemsSource = _News.Where(x => x.title.Trim().ToLower().Contains(tbox_searchbar.Text.Trim().ToLower()));
        }

        private void CustomCalendar_DisplayDateChanged(object? sender, CalendarDateChangedEventArgs e)
        {
            BrushesCalendar();
        }

        private void BrushesCalendar()
        {
            List<Workingcalendar> redDates = DBContext.Workingcalendars.Where(x => x.Isworkingday == false && x.Exceptiondate.Month == calendar_custom.DisplayDate.Month && x.Exceptiondate.Year == calendar_custom.DisplayDate.Year).ToList(); //Праздничные дни этого месяца
            List<Event> thisMonthEvents =_Events.Where(x => x.DatetimeStart.Month == calendar_custom.DisplayDate.Month && x.DatetimeStart.Year == calendar_custom.DisplayDate.Year ).ToList(); //События этого месяца
            List<Employee> bdayEmployees = _Employees.Where(x => x.Birthday.Month == calendar_custom.DisplayDate.Month).ToList(); //Именинники этого месяца
            DateTime dateNow = calendar_custom.DisplayDate;
            int dayCheck = 0; //индикатор для проверки на первое число в начале мемсяца


            foreach (var child in calendar_custom.GetVisualDescendants())
            {
                if (child is CalendarDayButton dayButton)
                {
                    string s1 = dayButton.Content.ToString().Replace("🎂", "");
                    dayButton.Content = s1;
                    dayButton.CornerRadius = new Avalonia.CornerRadius(5);
                    dayButton.Background = Brushes.Transparent;
                    dayButton.Foreground = Brushes.Black;
                    dayButton.IsVisible = true;
                    ToolTip.SetTip(dayButton, null);

                    string dayBtnContent = dayButton.Content!.ToString()!;

                    try
                    {
                        if (Convert.ToInt32(dayButton.Content) == 1)
                            dayCheck++;

                        if (dayCheck == 0 || dayCheck == 2)
                            dayButton.IsVisible = false;                        
                    

                        DateOnly nowDate = new DateOnly(dateNow.Year, dateNow.Month, int.Parse(dayBtnContent));

                        List<DateOnly> wCalendarDates = new();

                        //Выходные
                        foreach (Workingcalendar date in redDates)
                            wCalendarDates.Add(date.Exceptiondate);


                        //🎂
                        if (bdayEmployees.Where(x => x.Birthday.Day == nowDate.Day).Count() > 0)
                        {
                            string s = "";
                            ToolTip toolTip = new ToolTip();    
                            
                            dayButton.Content += "🎂";

                            foreach (Employee employee in bdayEmployees.ToList())
                                if (employee.Birthday.Day == nowDate.Day)
                                    s += $"{employee.Lastname}\n";

                            ToolTip.SetTip(dayButton, s);

                        }

                        //События
                        switch (thisMonthEvents.Where(x => x.DatetimeStart.Day == nowDate.Day).ToList().Count)
                        {
                            default:
                                dayButton.Background = Brushes.Red;
                                break;
                            case 1:
                            case 2:
                                dayButton.Background = Brushes.Green;
                                break;
                            case 3:
                            case 4:
                                dayButton.Background = Brushes.Yellow;
                                break;
                            case 0:
                                dayButton.Background = Brushes.Transparent;
                                break;
                        }

                        //Выходные (продолжение)
                        if (wCalendarDates.Contains(nowDate))
                        {
                            dayButton.BorderBrush = Brushes.DarkRed;
                            dayButton.BorderThickness = new Avalonia.Thickness(1);
                        }
                        else
                        {
                            dayButton.BorderBrush = Brushes.Transparent;
                            dayButton.BorderThickness = new Avalonia.Thickness(0);
                        }                        
                    }
                    catch
                    {
                        dayButton.BorderBrush = Brushes.Transparent;
                        dayButton.Background = Brushes.Transparent;
                        dayButton.Foreground = Brushes.Black;
                    }
                }
            }
        }

        private void OnCalendarLoaded(object sender, EventArgs e)
        {
            BrushesCalendar();
        }

        private void TextBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (tbox_searchbar != null) 
            {
                Searching();
            }
        }

        private void Searching()
        {
            if (tbox_searchbar.Text != "" && tbox_searchbar.Text != null )
            {
                string pattern = tbox_searchbar.Text.Trim().ToLower();
                _EmployeesFound.Clear();
                _EventsFound.Clear();
                _NewsFound.Clear();

                _EmployeesFound.AddRange(_Employees.Where(x =>
                x.Lastname.Trim().ToLower().Contains(pattern) ||
                x.Name.Trim().ToLower().Contains(pattern) ||
                x.Patronymic.Trim().ToLower().Contains(pattern) ||
                $"{x.Lastname} {x.Name} {x.Patronymic}".Contains(pattern) ||
                x.Job.Name.Trim().ToLower().Contains(pattern) ||
                x.Email.Trim().ToLower().Contains(pattern) ||
                x.Phone.Trim().ToLower().Contains(pattern) ||
                Convert.ToString(x.Birthday).Trim().ToLower().Contains(pattern)));

                _EventsFound.AddRange(_Events.Where(x =>
                x.Name.Trim().ToLower().Contains(pattern) ||
                x.Description.Trim().ToLower().Contains(pattern) ||
                x.IdOrganisatorNavigation.Lastname.Trim().ToLower().Contains(pattern) ||
                x.IdOrganisatorNavigation.Name.Trim().ToLower().Contains(pattern) ||
                x.IdOrganisatorNavigation.Patronymic.Trim().ToLower().Contains(pattern) ||
                $"{x.IdOrganisatorNavigation.Lastname} {x.IdOrganisatorNavigation.Name} {x.IdOrganisatorNavigation.Patronymic}".Contains(pattern) ||
                Convert.ToString(x.DatetimeStart).Trim().ToLower().Contains(pattern)));

                _NewsFound.AddRange(_News.Where(x =>
                x.title.Trim().ToLower().Contains(pattern) ||
                x.description.Trim().ToLower().Contains(pattern) ||
                x.date.Trim().ToLower().Contains(pattern)));


                lbox_employee.ItemsSource = _EmployeesFound.ToList();
                lbox_events.ItemsSource = _EventsFound.ToList();
                lbox_news.ItemsSource = _NewsFound.ToList();
            }
            else
            {
                lbox_employee.ItemsSource = _Employees.ToList();
                lbox_events.ItemsSource = _Events.ToList();
                NewsUpdate();
            }
        }
    }
}