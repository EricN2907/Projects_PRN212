using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBlogService
    {
        public List<Blog> GetAllBlogs();
        public Blog GetBlogById(int blogId);
        public Blog AddNewBlog(Blog newBlog);
        public Blog UpdateBlog(int blogId, Blog newBlog);
        public bool DeleteBlog(int blogId);

    }
}
