﻿using System;
using System.Collections.Generic;
using AutoGenerateForm.Attributes;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

namespace Medigenda
{
    public class Worker : Person
    {
        //Skills of a Worker are saved in a List where the a boolean give if the worker has this skill.
        private ObservableCollection<HaveSkills> skills = new ObservableCollection<HaveSkills>();

        //Available tima for the Worker - It's Legal, not mutabale
        private ObservableCollection<Tima> availableTima = new ObservableCollection<Tima> {
            new Tima("Full-Time", 1),
            new Tima("4/5", 4 / 5),
            new Tima("3/4", 3 / 4),
            new Tima("3/5", 3 / 5),
            new Tima("Half-Time", 1 / 2),
            new Tima("2/5", 2 / 5),
            new Tima("1/5", 1 / 5)
        };

        //Tima of the Worker
        private Tima tima;
        private List<DateTime> non_working_days = new List<DateTime>();

        //Dictionary containing all the days of an entire year. The value is 'true' if the employee is working that day and 'false' if he has a break 
        private Dictionary<DateTime, bool> working_days;
        private Dictionary<int, float> hours = new Dictionary<int, float>();
        //The key corresponds to the conversion of the DateTime attribute in the WorkingDay object
        private Dictionary<double, WorkingDay> schedule = new Dictionary<double, WorkingDay>();

        public Worker(string first, string last, int id) : base(first, last, id)
        {

            genDictDays(this.non_working_days);
                       
            //By default worker have Full-Time Tima
            this.tima = this.availableTima[0];
            this.skills = GenAvailableSkills();
         }

        #region Methods

        /* Cheks if the employee is working on the day 'date'
         * @pre - 
         * @post - 
         */
        public bool isWorking(DateTime date)
        {
            bool is_working = false;

            //Cheks if this date is included in the list "non_workings_days" of the worker
            foreach (KeyValuePair<double, WorkingDay> entry in this.schedule)
            {
                int result = DateTime.Compare(date, entry.Value.Date);
                if (result == 0)
                {
                    is_working = true;
                }
            }

            return is_working;
        }


        /* Cheks if the employee can work on the day "date" or not
         * @pre - working_days must be uptodate
         * @post - return 'true' or 'false' if the employe has a day break
         */ 
        public bool canWork(DateTime date)
        {
            return this.working_days[date];
        }


        public float getHours(DateTime date)
        {
            return -1;
        }

        /* Switches the current value of the DateTime in the dictionary 'working_days'. (By default all DateTime are 'true')
         * @pre -  
         * @post - a value 'true' becomes 'false' and vice versa
         */
        public void switchBreak(DateTime date)
        {
            this.working_days[date] =  !(this.working_days[date]);
        }

  

        /* Generates the dictionary "working_days" based on the list "non_working_days"
         * @pre - the list "non_working_days" must be uptodate
         * @post - the dictionary is created but the list "non_working_days" isn't changed
         */
         public void genDictDays(List<DateTime> breaks)
        {
            this.working_days = new Dictionary<DateTime, bool>();
            foreach(DateTime break_date in breaks)
            {
                this.working_days[break_date] = false;
            }
        }

        /*Updates its schedule by adding a new WorkingShift on a specific day "day"
         * @pre - the day must be in the dictionary "schedule"
         * @post - a new workingShift object is created for that day
         */
        public void updateSchedule(double date, ServiceName serv_name, double start_h, double end_h)
        {
            WorkingDay work_day;
            if(this.schedule.TryGetValue(date, out work_day))
            {
                work_day.addWorkingShift(serv_name, start_h, end_h);        
            }
            else
            {
                //DISPLAY ERROR???
            }
        } 

        /*Cheks if the worker is working during a specific period of time
         * @pre -
         * @post -
         */
         public bool isFree(double start_h, double end_h)
        {
            return true;
        }



        public ObservableCollection<HaveSkills> GenAvailableSkills()
        {

            //Remove and update when DB is Available
            //Needed for test
            return new ObservableCollection<HaveSkills>
            { 
            new HaveSkills(new ServiceName("CT-Scan")),
            new HaveSkills(new ServiceName("Radio")),
            new HaveSkills(new ServiceName("IRM")),
            new HaveSkills(new ServiceName("Mammo")),
            new HaveSkills(new ServiceName("URG"))
            };
        }



        #endregion

        #region Properties
        [AutoGenerateProperty]
        [DisplayMemberPathCollection("Name")]
        [SelectedItemCollection("Tima")]
        [Display("Time Schedule")]
        [PropertyOrder(4)]
        public ObservableCollection<Tima> AvailableTima
        {
            get { return this.availableTima; }
            set { this.availableTima = value; NotifyPropertyChanged(); }
        }


        public Tima Tima
        {
            get { return this.tima; }
            set { this.tima = value; NotifyPropertyChanged(); }
        }
        

        public ObservableCollection<HaveSkills> Skills
        {
            get { return this.skills;}
            set { this.skills = value; }
        }

        public List<DateTime> Non_working_days
        {
            get { return this.non_working_days; }
        }
        #endregion
    }

}