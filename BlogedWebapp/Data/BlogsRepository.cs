﻿using BlogedWebapp.Entities;
using BlogedWebapp.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogedWebapp.Data
{

    public interface IBlogsRepository : IGenericRepository<Blog>
    {

        Task<UsersBlog> SetBlogOwner(Blog blog, AppUser user);

        Task<Blog> GetByUrlName(string urlName, ProjectionBehaviour projectionBehaviour = ProjectionBehaviour.Default);
    }

    public class BlogsRepository : GenericRepository<Blog>, IBlogsRepository
    {
        public BlogsRepository(
                DataContext context,
                ILogger logger
            ) : base(context, logger)
        {


        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Blog>> All(ProjectionBehaviour projectionBehaviour)
        {
            try
            {
                return await MakeQueryProjection(projectionBehaviour)
                                .Where(x => x.Status == 1)
                                .AsNoTracking()
                                .ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"All\" method has generated an error.", typeof(BlogsRepository));
                return new List<Blog>();
            }
        }

        /// <inheritdoc/>
        public override async Task<Blog> GetById(Guid Id, ProjectionBehaviour projectionBehaviour)
        {
            try
            {

                return await MakeQueryProjection(projectionBehaviour)
                            .FirstOrDefaultAsync(u => u.Id == Id.ToString());

            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"GetById\" method has generated an error.", typeof(BlogsRepository));
                return new Blog();
            }
        }

        public async Task<Blog> GetByUrlName(string urlName, ProjectionBehaviour projectionBehaviour)
        {
            try
            {
                return await MakeQueryProjection(projectionBehaviour)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.UrlName.Equals(urlName));
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"GetByUrlName\" method has generated an error.", typeof(BlogsRepository));
                return new Blog();
            }
        }

        public async Task<UsersBlog> SetBlogOwner(Blog blog, AppUser user)
        {
            try
            {
                UsersBlog usersBlog = new UsersBlog()
                {
                    Blog = blog,
                    Owner = user,
                    Role = BlogRoles.Owner
                };

                await this.dataContext.Set<UsersBlog>().AddAsync(usersBlog);

                return usersBlog;
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"SetBlogOwner\" method has generated an error.", typeof(BlogsRepository));
                return new UsersBlog();
            }

        }

        public override async Task<bool> Update(Blog blog)
        {
            try
            {
                await dbSet
                        .Include(u => u.UsersBlog)
                        .FirstOrDefaultAsync(u => u.Id == blog.Id);

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"Update\" method has generated an error.", typeof(BlogsRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(Blog blog)
        {
            try
            {
                var blogObj = await dbSet
                        .FirstOrDefaultAsync(u => u.Id == blog.Id);
                dbSet.Remove(blogObj);

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Repo} \"Delete\" method has generated an error.", typeof(BlogsRepository));
                return false;
            }
        }

        /// <inheritdoc/>
        public override IQueryable<Blog> MakeQueryProjection(ProjectionBehaviour projectionBehaviour)
        {
            IQueryable<Blog> queryable = null;

            switch (projectionBehaviour)
            {
                case ProjectionBehaviour.Minimal:
                    queryable = dbSet.Select(b => new Blog()
                    {
                        Title = b.Title,
                        UrlName = b.UrlName,
                        Description = b.Description,
                        Id = b.Id,
                        Status = b.Status,
                        CreatedOn = b.CreatedOn,
                        UpdatedOn = b.UpdatedOn
                    });
                    break;


                case ProjectionBehaviour.Default:
                    queryable = dbSet;
                    break;

                case ProjectionBehaviour.IncludeRelated:
                    queryable = dbSet
                                .Include(u => u.UsersBlog)
                                .Include(u => u.Posts);
                    break;

                case ProjectionBehaviour.IncludeRecursively:
                    queryable = dbSet
                                .Include(u => u.UsersBlog)
                                .Include(u => u.Posts);
                    break;
            }

            return queryable;
        }
    }
}