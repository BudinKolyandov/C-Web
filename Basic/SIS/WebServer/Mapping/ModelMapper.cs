using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SIS.MvcFramework.Mapping
{
    public static class ModelMapper
    {
        private static object MapProperty(object origin, Type destinationType)
        {
            var destinationInstance = Activator.CreateInstance(destinationType);

            foreach (var originProperty in origin.GetType().GetProperties())
            {
                string propertyName = originProperty.Name;
                PropertyInfo destinationProperty = destinationInstance
                    .GetType().GetProperty(propertyName);

                if (destinationProperty != null)
                {
                    if (destinationProperty.PropertyType == typeof(string))
                    {
                        destinationProperty
                            .SetValue(destinationInstance,
                            originProperty.GetValue(origin).ToString());
                    }
                    else
                    {
                        destinationProperty.SetValue(destinationInstance,
                            originProperty.GetValue(origin));
                    }
                }
            }

            return destinationInstance;
        }


        public static TDestination ProjectTo<TDestination>(object origin)
        {
            var destinationInstance = (TDestination)Activator.CreateInstance(typeof(TDestination));

            foreach (var originProperty in origin.GetType().GetProperties())
            {
                string propertyName = originProperty.Name;
                PropertyInfo destinationProperty = destinationInstance
                    .GetType().GetProperty(propertyName);

                if (destinationProperty != null)
                {
                    if (destinationProperty.PropertyType == typeof(string))
                    {
                        destinationProperty
                            .SetValue(destinationInstance, 
                            originProperty.GetValue(origin).ToString());
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType))
                    {
                        var originCollection = (IEnumerable)originProperty.GetValue(origin);
                        var destinationElementType = destinationProperty
                            .GetValue(destinationInstance)
                            .GetType()
                            .GetGenericArguments()[0];
                        var destinationCollection = (IList)Activator.CreateInstance(destinationProperty.PropertyType);

                        foreach (var originElement in originCollection)
                        {
                            destinationCollection.Add(MapProperty(originElement, destinationElementType));
                        }
                        destinationProperty.SetValue(destinationInstance, destinationCollection);
                    }
                    else
                    {
                        destinationProperty.SetValue(destinationInstance,
                            originProperty.GetValue(origin));
                    }
                }
            }

            return destinationInstance;
        }

    }
}
