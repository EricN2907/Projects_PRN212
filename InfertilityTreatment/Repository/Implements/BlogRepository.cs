using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class BlogRepository : IBlogRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public BlogRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public BlogRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<Blog> GetAllBlogs()
        {
            return _context.Blogs.ToList();
        }

        public Blog GetBlogById(int blogId)
        {
            var blogs = _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);

            return blogs;
        }

        public Blog AddNewBlog(Blog newBlog)
        {
            if (newBlog == null)
            {
                return null;
            }
            _context.Blogs.Add(newBlog);
            _context.SaveChanges();
            return newBlog;
        }

        public Blog UpdateBlog(int blogId, Blog newBlog)
        {

            var existingBlog = _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
            if (existingBlog == null)
            {
                return null;
            }
            existingBlog.Title = newBlog.Title;
            existingBlog.Content = newBlog.Content;
            existingBlog.CreatedDate = DateTime.Now; // Update to current date

            _context.SaveChanges();
            return existingBlog;
        }

        public bool DeleteBlog(int blogId)
        {
            var existingBlog = _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
            if (existingBlog == null)
            {
                return false;
            }
            _context.Blogs.Remove(existingBlog);
            _context.SaveChanges();
            return true;
        }
    }
}
