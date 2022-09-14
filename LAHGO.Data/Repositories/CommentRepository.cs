using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
