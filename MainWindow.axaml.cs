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
        }

        private void NewsUpdate()
        {
            _News.Clear();
            //_News = JsonConvert.DeserializeObject<List<News>>(File.ReadAllText("Assets/news_response.json"));
            lbox_news.ItemsSource = _News.ToList();
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


            foreach (var child in calendar_custom.GetVisualDescendants())
            {
                if (child is CalendarDayButton dayButton)
                {
                    dayButton.Background = Brushes.Transparent;
                    dayButton.Foreground = Brushes.Black;

                    string dayBtnContent = dayButton.Content!.ToString()!;

                    try
                    {
                        DateOnly nowDate = new DateOnly(dateNow.Year, dateNow.Month, int.Parse(dayBtnContent));

                        List<DateOnly> wCalendarDates = new();

                        foreach (Workingcalendar date in redDates)
                            wCalendarDates.Add(date.Exceptiondate);


                        //🎂
                        if (bdayEmployees.Where(x => x.Birthday.Day == nowDate.Day).Count() > 0)
                        {
                            dayButton.Content += "🎂";

                            Flyout F = new Flyout();

                            foreach (Employee employee in bdayEmployees.ToList())
                                if (employee.Birthday.Day == nowDate.Day)
                                    F.Content += $"{employee.Lastname} {employee.Name} {employee.Patronymic} \n";

                            dayButton.Flyout = F;
                        }

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
    }
}