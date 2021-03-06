﻿using Eagle.Core;
using Eagle.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Application
{
    public abstract class ApplicationService : DisposableObject
    {
        private IRepositoryContext repositoryContext;

        public ApplicationService(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        protected IRepositoryContext RepositoryContext 
        {
            get 
            {
                return this.repositoryContext;
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.RepositoryContext.Dispose();
        }
    }
}
