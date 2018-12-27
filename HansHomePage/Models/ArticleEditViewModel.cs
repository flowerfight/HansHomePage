using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HansHomePage.Models
{
    public class ArticleEditViewModel
    {
        public Articles Articles { get; set; }

        public List<Articles> ArticlesList { get; set; }

        public List<ArticleFiles> Files { get; set; }

        public List<ArticleComments> Comments { get; set; }
    }
}