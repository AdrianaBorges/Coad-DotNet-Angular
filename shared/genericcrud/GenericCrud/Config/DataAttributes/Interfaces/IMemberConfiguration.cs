using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Interfaces
{
    public interface IMemberConfiguration : IMemberConfigurationExpression
    {
        // Summary:
        //     Ignore this member for configuration validation and skip during mapping
        void Ignore();
        //
        // Summary:
        //     Map from a specific source member
        //
        // Parameters:
        //   sourceMember:
        //     Source member to map from
        void MapFrom(string sourceMember);
        //
        // Summary:
        //     Resolve destination member using a custom value resolver
        //
        // Type parameters:
        //   TValueResolver:
        //     Value resolver of type AutoMapper.IValueResolver
        //
        // Returns:
        //     Value resolver configuration options
        IResolverConfigurationExpression ResolveUsing<TValueResolver>();
        //
        // Summary:
        //     Resolve destination member using a custom value resolver instance
        //
        // Parameters:
        //   valueResolver:
        //     Value resolver to use
        //
        // Returns:
        //     Value resolver configuration options
        IResolutionExpression ResolveUsing(IValueResolver valueResolver);
        //
        // Summary:
        //     Resolve destination member using a custom value resolver
        //
        // Parameters:
        //   valueResolverType:
        //     Value resolver of type AutoMapper.IValueResolver
        //
        // Returns:
        //     Value resolver configuration options
        IResolverConfigurationExpression ResolveUsing(Type valueResolverType);
        //
        // Summary:
        //     Use the destination value instead of mapping from the source value or creating
        //     a new instance
        void UseDestinationValue();
    }
}
