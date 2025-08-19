using BusinessObject.Models;
using Repository.Implements;
using Repository.Interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implements
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService()
        {
            _blogRepository = new BlogRepository();
        }
        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public Blog AddNewBlog(Blog newBlog)
        {
            return _blogRepository.AddNewBlog(newBlog);
        }

        public bool DeleteBlog(int blogId)
        {
            return _blogRepository.DeleteBlog(blogId);
        }

        public List<Blog> GetAllBlogs()
        {
            return _blogRepository.GetAllBlogs();
        }

        public Blog GetBlogById(int blogId)
        {
            return _blogRepository.GetBlogById(blogId);
        }

        public Blog UpdateBlog(int blogId, Blog newBlog)
        {
            return _blogRepository.UpdateBlog(blogId, newBlog);
        }
    }
}
