using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class PostLike
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string UserId { get; set; }

    }
}
