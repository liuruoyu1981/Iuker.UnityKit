using System;

namespace Iuker.Common.Base
{
    public class CreateDescAttribute : Attribute
    {
        public string Author { get; private set; }

        public string CreateDate { get; private set; }

        public string Email { get; private set; }


        public CreateDescAttribute(string author, string date, string email)
        {
            Author = author;
            CreateDate = date;
            Email = email;
        }

    }
}