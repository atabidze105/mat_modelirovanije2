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
            foreach (var child in calendar_custom.GetVisualDescendants())
            {
                if (child is CalendarDayButton dayButton)
                {
                    var dateNow = (calendar_custom as Calendar).DisplayDate;

                    string vv = dayButton.Content!.ToString()!;


                    try
                    {
                        DateOnly nowDate = new DateOnly(dateNow.Year, dateNow.Month, int.Parse(vv));

                        List<DateOnly> wCalendarDates = new();

                        foreach (Workingcalendar date in DBContext.Workingcalendars.Where(x => x.Isworkingday == false).ToList())
                            wCalendarDates.Add(date.Exceptiondate);

                        if (wCalendarDates.Contains(nowDate))
                        {
                            //🎂
                            if (_Employees.Where(x => x.Birthday.Month == nowDate.Month && x.Birthday.Day == nowDate.Day).ToList().Count() > 0)
                            {
                                dayButton.Content += "🎂";

                                Flyout F = new Flyout();

                                foreach (Employee employee in _Employees.Where(x => x.Birthday.Month == nowDate.Month && x.Birthday.Day == nowDate.Day).ToList())
                                    F.Content += $"{employee.Lastname} {employee.Name} {employee.Patronymic} \n";

                                dayButton.Flyout = F;
                            }

                            dayButton.Flyout = new Flyout { Content = "" };
                            dayButton.Background = Brushes.Red;
                        }
                        else
                        {
                            dayButton.Background = Brushes.LightGray;
                            dayButton.Foreground = Brushes.Black;
                        }
                    }
                    catch
                    {
                        dayButton.Background = Brushes.LightGray;
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