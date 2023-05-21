using System.ComponentModel;

namespace Movies.Api.Models
{
    public class SearchModel
    {
        [DisplayName("Search")]
        public string ListName { get; set; }
    }
}