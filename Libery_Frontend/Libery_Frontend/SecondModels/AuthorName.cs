using System;
using System.Collections.Generic;
using System.Text;

namespace Libery_Frontend.Views
{
    public class AuthorName
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int AuthorID { get; set; }
        public override string ToString() => $"{Firstname} {Lastname}";



    }
}