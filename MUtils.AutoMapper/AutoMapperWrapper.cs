﻿using System;
using AutoMapper;
using IObjectMapper = MUtils.Interface.IObjectMapper;

namespace MUtils.AutoMapper
{
    public class AutoMapperWrapper : IObjectMapper
    {
        private readonly IMapper _mapper = null;
        public AutoMapperWrapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

    }
}
