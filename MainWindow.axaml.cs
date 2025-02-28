using Avalonia.Controls;
using Avalonia.Threading;
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
        }

        private void NewsUpdate()
        {
            _News.Clear();
            _News = JsonConvert.DeserializeObject<List<News>>(File.ReadAllText("Assets/news_response.json"));
            lbox_news.ItemsSource = _News.ToList();
        }
    }
}