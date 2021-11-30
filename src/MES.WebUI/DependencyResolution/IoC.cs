// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace MES.WebUI.DependencyResolution {
    using FluentValidation;
    using MediatR;
    using MediatR.Pipeline;
    using MES.Core.Features.ViewProcessOrder;
    using MES.Core.Infrastructure.Validation;
    using MES.Persistence;
    using StructureMap;
    using System.Configuration;

    public static class IoC {
        public static IContainer Initialize() {
            return new Container(c => {
                c.AddRegistry<DefaultRegistry>();

                // Add the following for mediator
                c.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<SearchQuery>(); // Our assembly with requests & handlers
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.AddAllTypesOf(typeof(IRequestPreProcessor<>));
                    scanner.AddAllTypesOf(typeof(IValidator<>));
                });

                //Pipeline
                c.For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
                c.For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));
                c.For(typeof(IRequestPreProcessor<>)).Add(typeof(ValidationProcessorBehavior<>));
                c.For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
                c.For<IMediator>().Use<Mediator>();
                c.ForConcreteType<MESDbContext>().Configure
                   // StructureMap parses the expression passed
                   // into the method below to determine the
                   // constructor
                   .SelectConstructor(() => new MESDbContext());

                // Validation
                c.For(typeof(IMessageValidator<>)).Use(typeof(FluentValidationMessageValidator<>));
            });
        }
    }
}