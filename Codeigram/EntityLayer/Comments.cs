using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Comments
    {
        public int Id { get; set; }
        public string Usernmame { get; set; }
        public string UserId { get; set; }
        public string UserImg { get; set; }
        public string Comment { get; set; }
        public DateTime Created_at { get; set; }
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public int Status { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }






    }
}
