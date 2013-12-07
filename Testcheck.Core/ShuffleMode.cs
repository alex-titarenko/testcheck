using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TAlex.Testcheck.Core
{
    public enum ShuffleMode
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "All")]
        All,
        [Display(Name = "Except last")]
        ExceptLast,
        [Display(Name = "Except two last")]
        ExceptTwoLast
    }
}
