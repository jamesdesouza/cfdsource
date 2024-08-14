using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;

namespace TCX.CFD.Classes.FlowDesigner;

internal class FlowDesignerSerializationService : IDesignerSerializationService
{
	private readonly IServiceProvider provider;

	public FlowDesignerSerializationService(IServiceProvider provider)
	{
		this.provider = provider;
	}

	public ICollection Deserialize(object serializationData)
	{
		if (!(serializationData is SerializationStore store))
		{
			return Array.Empty<object>();
		}
		return (provider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService).Deserialize(store);
	}

	public object Serialize(ICollection objects)
	{
		ComponentSerializationService componentSerializationService = provider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
		using SerializationStore serializationStore = componentSerializationService.CreateStore();
		foreach (object @object in objects)
		{
			if (@object is Activity)
			{
				componentSerializationService.Serialize(serializationStore, @object);
			}
		}
		return serializationStore;
	}
}
