using Client.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        IMemoAPI memoApi;
        public Form1()
        {
            InitializeComponent();
            memoApi = RestService.For<IMemoAPI>(ConfigurationSettings.AppSettings["MemoApiURL"]);
        }
    }
}
