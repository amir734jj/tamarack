using System;

namespace Tamarack
{
	public class ActivatorServiceProvider : ITamarackServiceProvider
    {
		public object GetService(Type serviceType)
		{
			return Activator.CreateInstance(serviceType);
		}
	}
}