using Api.CrossCutting.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Test.Service
{
    public abstract class BaseTestService
    {
        public IMapper Mapper { get; set; }

        public BaseTestService()
        {
            Mapper = new AutoMapperFixture().GetMapper();
        }

        public class AutoMapperFixture : IDisposable
        {
            public IMapper GetMapper()
            {
                var config = new MapperConfiguration(config =>
                {
                    config.AddProfile(new ModelToEntityProfile());
                    config.AddProfile(new EntityToDtoProfile());
                    config.AddProfile(new DtoToModelProfile());
                });

                return config.CreateMapper();
            }

            public void Dispose()
            {
                
            }
        }
    }
}
