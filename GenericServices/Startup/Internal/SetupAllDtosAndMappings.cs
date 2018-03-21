﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using GenericServices.Configuration;
using GenericServices.Configuration.Internal;
using GenericServices.Internal.Decoders;
using GenericServices.PublicButHidden;

namespace GenericServices.Startup.Internal
{
    internal class SetupAllDtosAndMappings : StatusGenericHandler
    {
        private readonly IExpandedGlobalConfig _configuration;

        WrappedAutoMapperConfig AutoMapperConfig { get; set; }

        public SetupAllDtosAndMappings(IExpandedGlobalConfig configuration, Assembly[] assembliesToScan)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            //Add ForAllPropertyMaps to start the Mapping
            foreach (var assembly in assembliesToScan)
            {
                RegisterDtosInAssemblyAndBuildMaps(assembly);
            }
        }


        public void RegisterDtosInAssemblyAndBuildMaps(Assembly assemblyToScan)
        {
            Header = $"Scanning {assemblyToScan.GetName()}";
            var allTypesInAssembly = assemblyToScan.GetTypes();
            var allLinkToEntityClasses = allTypesInAssembly
                .Where(x => x.GetLinkedEntityFromDto() != null);

            foreach (var dtoType in allLinkToEntityClasses)
            {
                var register = new RegisterOneDtoType(dtoType, _configuration);
                if (!register.IsValid)
                {
                    CombineStatuses(register);
                    continue;
                }

                //Now build the mapping using the MapGenerator in the register
            }

            //Now scan for next maps and set up the mapping for them too
            throw new NotImplementedException();
            //Don't forget to look at the TurnOffAuthoMapperSaveFilter in the GenericServicesConfig
        }




    }
}