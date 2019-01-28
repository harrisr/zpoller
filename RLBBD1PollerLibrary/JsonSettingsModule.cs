//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLBBD1PollerLibrary
//{
//    public class JsonSettingsModule : Module
//    {
//        private readonly string _configurationFilePath;
//        private readonly string _sectionNameSuffix;

//        public JsonSettingsModule(string configurationFilePath, string sectionNameSuffix = "Settings")
//        {
//            _configurationFilePath = configurationFilePath;
//            _sectionNameSuffix = sectionNameSuffix;
//        }

//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterInstance(new Settings.SettingsReader(_configurationFilePath, _sectionNameSuffix))
//                .As<ISettingsReader>()
//                .SingleInstance();

//            var settings = Assembly.GetExecutingAssembly()
//                .GetTypes()
//                .Where(t => t.Name.EndsWith(_sectionNameSuffix, StringComparison.InvariantCulture))
//                .ToList();

//            settings.ForEach(type =>
//            {
//                builder.Register(c => c.Resolve<ISettingsReader>().LoadSection(type))
//                    .As(type)
//                    .SingleInstance();
//            });
//        }
//    }
//}




