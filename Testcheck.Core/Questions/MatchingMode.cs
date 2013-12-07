using System.ComponentModel.DataAnnotations;


namespace TAlex.Testcheck.Core.Questions
{
    public enum MatchingMode
    {
        [Display(Name = "One to One")]
        OneToOne,
        [Display(Name = "Many to One")]
        ManyToOne,
        [Display(Name = "One to Many")]
        OneToMany,
        [Display(Name = "Many to Many")]
        ManyToMany
    }
}
