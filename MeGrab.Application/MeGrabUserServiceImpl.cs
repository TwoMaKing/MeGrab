using Eagle.Core.QuerySepcifications;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Application
{
    public class MeGrabUserServiceImpl : ApplicationService, IMeGrabUserService
    {
        private IMeGrabUserRepository userRepository;

        public MeGrabUserServiceImpl(IRepositoryContext repositoryContext,
                                     IMeGrabUserRepository userRepository)
            : base(repositoryContext)
        {
            this.userRepository = userRepository;
        }

        public MeGrabUserDataObject GetRegisteredUserById(int id)
        {
            MeGrabUser registeredUser = userRepository.FindByKey(id);

            if (registeredUser == null)
            {
                return null;
            }

            MeGrabUserDataObject registeredUserDataObject = new MeGrabUserDataObject();
            registeredUserDataObject.MapFrom(registeredUser);

            return registeredUserDataObject;
        }

        public MeGrabUserDataObject GetRegisteredUserByName(string name)
        {
            ISpecification<MeGrabUser> spec = new ExpressionSpecification<MeGrabUser>(u => u.Name == name);

            MeGrabUser registeredUser = userRepository.Find(spec);

            if (registeredUser == null)
            {
                return null;
            }

            MeGrabUserDataObject registeredUserDataObject = new MeGrabUserDataObject();
            registeredUserDataObject.MapFrom(registeredUser);

            return registeredUserDataObject;
        }
    }
}
