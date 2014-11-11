// Cinema Suite 2014
using System;

namespace CinemaDirector
{
    public class CutsceneItemAttribute : Attribute
    {
        private string category;
        private string label;

        public CutsceneItemAttribute(string category, string label)
        {
            this.category = category;
            this.label = label;
        }

        public string Category
        {
            get
            {
                return category;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
        }
    }
}