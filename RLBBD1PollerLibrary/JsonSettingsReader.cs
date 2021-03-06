﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace RLBBD1PollerLibrary
{
    public class JsonSettingsReader : IJsonSettingsReader
    {
        private readonly string _configurationFilePath;
        private readonly string _sectionNameSuffix;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new SettingsReaderContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public JsonSettingsReader(string configurationFilePath, string sectionNameSuffix = "Settings")
        {
            _configurationFilePath = configurationFilePath;
            _sectionNameSuffix = sectionNameSuffix;
        }

        public T Load<T>() where T : class, new() => Load(typeof(T)) as T;

        public T LoadSection<T>() where T : class, new() => LoadSection(typeof(T)) as T;

        public object Load(Type type)
        {
            if (!File.Exists(_configurationFilePath))
                return Activator.CreateInstance(type);

            var jsonFile = File.ReadAllText(_configurationFilePath);

            return JsonConvert.DeserializeObject(jsonFile, type, JsonSerializerSettings);
        }

        public object LoadSection(Type type)
        {
            if (!File.Exists(_configurationFilePath))
                return Activator.CreateInstance(type);

            var jsonFile = File.ReadAllText(_configurationFilePath);
            var section = ToCamelCase(type.Name.Replace(_sectionNameSuffix, string.Empty));
            var settingsData = JsonConvert.DeserializeObject<dynamic>(jsonFile, JsonSerializerSettings);
            var settingsSection = settingsData[section];

            return settingsSection == null
                ? Activator.CreateInstance(type)
                : JsonConvert.DeserializeObject(JsonConvert.SerializeObject(settingsSection), type,
                    JsonSerializerSettings);
        }

        private class SettingsReaderContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(f => CreateProperty(f, memberSerialization)))
                    .ToList();
                props.ForEach(p =>
                {
                    p.Writable = true;
                    p.Readable = true;
                });

                return props;
            }
        }

        private static string ToCamelCase(string text)
            => string.IsNullOrWhiteSpace(text)
                ? string.Empty
                : $"{text[0].ToString().ToLowerInvariant()}{text.Substring(1)}";
    }
}







