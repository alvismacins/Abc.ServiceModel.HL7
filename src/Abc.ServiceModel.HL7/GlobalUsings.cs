#if CoreWCF
global using CoreWCF;
global using CoreWCF.Channels;
global using CoreWCF.Description;
global using CoreWCF.Dispatcher;
#else
global using System.ServiceModel;
global using System.ServiceModel.Channels;
global using System.ServiceModel.Description;
global using System.ServiceModel.Dispatcher;
#endif