
// This file contains registration of aspects that are applied to several classes of this project.

using System;
using Mono.Logging;
using PostSharp.Extensibility;

/*[assembly:
ExceptionWrapper(AttributeTargetMembers = "Mono.Logging.Calculator.*",
    AttributeTargetElements = MulticastTargets.Method, 
    AttributeTargetTypeAttributes = MulticastAttributes.Public, 
    AttributeTargetMemberAttributes = MulticastAttributes.Public)]*/