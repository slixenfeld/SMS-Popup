﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SMS_Popup
{
    public partial class Popup : Window
    {
        public Popup()
        {
            InitializeComponent();
            new SMSRestClient(this).RequestLastSMS();
            Top = 20;
            Left = 20;
            Show();
        }
    }
}
