﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorCamadaCSharp.Library
{
    public class ArquivoBaseInfo
    {
        public static string RetornaTextoArquivo(string pacote)
        {
            StringBuilder funcoes = new StringBuilder();
            funcoes.AppendLine("using Newtonsoft.Json;                                                                                                                                                                                          ");
            funcoes.AppendLine("using System;                                                                                                                                                                                                   ");
            funcoes.AppendLine("using System.Collections;                                                                                                                                                                                       ");
            funcoes.AppendLine("using System.Collections.Concurrent;                                                                                                                                                                            ");
            funcoes.AppendLine("using System.Collections.Generic;                                                                                                                                                                               ");
            funcoes.AppendLine("using System.ComponentModel;                                                                                                                                                                                    ");
            funcoes.AppendLine("using System.Diagnostics;                                                                                                                                                                                       ");
            funcoes.AppendLine("using System.Linq.Expressions;                                                                                                                                                                                  ");
            funcoes.AppendLine("using System.Reflection;                                                                                                                                                                                        ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("namespace " + pacote + ".BaseObjects                                                                                                                                                                       ");
            funcoes.AppendLine("{                                                                                                                                                                                                               ");
            funcoes.AppendLine("    public enum DataObjectStateEnum { None, Added, Confirmed, Modified, Deleted }                                                                                                                               ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("    [Serializable]                                                                                                                                                                                              ");
            funcoes.AppendLine("    public class BaseInfo : IDisposable, INotifyPropertyChanged                                                                                                                                                 ");
            funcoes.AppendLine("    {                                                                                                                                                                                                           ");
            funcoes.AppendLine("        ConcurrentDictionary<string, PropertyChangedEventArgs> eventArgumentCache;                                                                                                                              ");
            funcoes.AppendLine("        static Dictionary<Type, PropertyDescriptorCollection> propertiesCache = new Dictionary<Type, PropertyDescriptorCollection>();                                                                           ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        [JsonIgnore]                                                                                                                                                                                            ");
            funcoes.AppendLine("        public DataObjectStateEnum DataObjectState { get; set; }                                                                                                                                                ");
            funcoes.AppendLine("        [JsonIgnore]                                                                                                                                                                                            ");
            funcoes.AppendLine("        public string InclusaoUsuario { get; set; }                                                                                                                                                             ");
            funcoes.AppendLine("        [JsonIgnore]                                                                                                                                                                                            ");
            funcoes.AppendLine("        public DateTime InclusaoData { get; set; }                                                                                                                                                              ");
            funcoes.AppendLine("        [JsonIgnore]                                                                                                                                                                                            ");
            funcoes.AppendLine("        public string AlteracaoUsuario { get; set; }                                                                                                                                                            ");
            funcoes.AppendLine("        [JsonIgnore]                                                                                                                                                                                            ");
            funcoes.AppendLine("        public DateTime AlteracaoData { get; set; }                                                                                                                                                             ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        public BaseInfo()                                                                                                                                                                                       ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            eventArgumentCache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();                                                                                                                  ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("            InclusaoData = DateTime.Now;                                                                                                                                                                        ");
            funcoes.AppendLine("            AlteracaoData = DateTime.Now;                                                                                                                                                                       ");
            funcoes.AppendLine("            DataObjectState = DataObjectStateEnum.Added;                                                                                                                                                        ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        [field: NonSerialized]                                                                                                                                                                                  ");
            funcoes.AppendLine("        public event PropertyChangedEventHandler PropertyChanged;                                                                                                                                               ");
            funcoes.AppendLine("        protected virtual void OnPropertyChanged(string propertyName)                                                                                                                                           ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            EnforceChangedPropertyExists(propertyName);                                                                                                                                                         ");
            funcoes.AppendLine("            PropertyChangedEventHandler copy = PropertyChanged;                                                                                                                                                 ");
            funcoes.AppendLine("            if (copy != null)                                                                                                                                                                                   ");
            funcoes.AppendLine("            {                                                                                                                                                                                                   ");
            funcoes.AppendLine("                if (DataObjectState == DataObjectStateEnum.Confirmed)                                                                                                                                           ");
            funcoes.AppendLine("                    DataObjectState = DataObjectStateEnum.Modified;                                                                                                                                             ");
            funcoes.AppendLine("                copy(this, GetOrCreatePropertyChangedEventArgs(propertyName));                                                                                                                                  ");
            funcoes.AppendLine("            }                                                                                                                                                                                                   ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)                                                                                                                       ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            var lambda = (LambdaExpression)property;                                                                                                                                                            ");
            funcoes.AppendLine("            MemberExpression memberExpression;                                                                                                                                                                  ");
            funcoes.AppendLine("            {                                                                                                                                                                                                   ");
            funcoes.AppendLine("                var unaryExpression = lambda.Body as UnaryExpression;                                                                                                                                           ");
            funcoes.AppendLine("                memberExpression = (unaryExpression != null) ? (MemberExpression)unaryExpression.Operand : (MemberExpression)lambda.Body;                                                                       ");
            funcoes.AppendLine("            }                                                                                                                                                                                                   ");
            funcoes.AppendLine("            OnPropertyChanged(memberExpression.Member.Name);                                                                                                                                                    ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        private PropertyChangedEventArgs GetOrCreatePropertyChangedEventArgs(string propertyName)                                                                                                               ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            PropertyChangedEventArgs arguments;                                                                                                                                                                 ");
            funcoes.AppendLine("            if (eventArgumentCache.TryGetValue(propertyName, out arguments))                                                                                                                                    ");
            funcoes.AppendLine("                return arguments;                                                                                                                                                                               ");
            funcoes.AppendLine("            return eventArgumentCache.GetOrAdd(propertyName, new PropertyChangedEventArgs(propertyName));                                                                                                       ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        [Conditional(\"DEBUG\")]                                                                                                                                                                                ");
            funcoes.AppendLine("        void EnforceChangedPropertyExists(string propertyName)                                                                                                                                                  ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            PropertyInfo property = GetType().GetProperty(propertyName);                                                                                                                                        ");
            funcoes.AppendLine("            if (property == null)                                                                                                                                                                               ");
            funcoes.AppendLine("                throw new ArgumentException(string.Format(\"Type '{0}' tried to raise a change notifcation for property '{1}', but no such property exists!\", GetType().Name, propertyName), \"propertyName\");");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        PropertyDescriptorCollection GetProperties()                                                                                                                                                            ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            Type type = this.GetType();                                                                                                                                                                         ");
            funcoes.AppendLine("            if (!propertiesCache.ContainsKey(type))                                                                                                                                                             ");
            funcoes.AppendLine("                propertiesCache[type] = TypeDescriptor.GetProperties(this);                                                                                                                                     ");
            funcoes.AppendLine("            return propertiesCache[type];                                                                                                                                                                       ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        void CopySubClass<T>(PropertyDescriptor property, object value, T to)                                                                                                                                   ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            object objectTo = property.GetValue(to);                                                                                                                                                            ");
            funcoes.AppendLine("            if (objectTo != null)                                                                                                                                                                               ");
            funcoes.AppendLine("                ((BaseInfo)value).CopyTo(objectTo);                                                                                                                                                             ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        public void CopyTo<T>(T to)                                                                                                                                                                             ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            PropertyDescriptorCollection properties = GetProperties();                                                                                                                                          ");
            funcoes.AppendLine("            foreach (PropertyDescriptor property in properties)                                                                                                                                                 ");
            funcoes.AppendLine("            {                                                                                                                                                                                                   ");
            funcoes.AppendLine("                if (!property.IsReadOnly)                                                                                                                                                                       ");
            funcoes.AppendLine("                {                                                                                                                                                                                               ");
            funcoes.AppendLine("                    object value = property.GetValue(this);                                                                                                                                                     ");
            funcoes.AppendLine("                    if (property.PropertyType.IsSubclassOf(typeof(BaseInfo)) && value != null)                                                                                                                  ");
            funcoes.AppendLine("                        CopySubClass(property, (BaseInfo)value, to);                                                                                                                                            ");
            funcoes.AppendLine("                    else if (value != null && !(value is ICollection))                                                                                                                                          ");
            funcoes.AppendLine("                        property.SetValue(to, value);                                                                                                                                                           ");
            funcoes.AppendLine("                }                                                                                                                                                                                               ");
            funcoes.AppendLine("            }                                                                                                                                                                                                   ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        ~BaseInfo()                                                                                                                                                                                             ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            FinalizeObject(_disposing: false);                                                                                                                                                                  ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        public void Dispose()                                                                                                                                                                                   ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            FinalizeObject(_disposing: true);                                                                                                                                                                   ");
            funcoes.AppendLine("            GC.SuppressFinalize(this);                                                                                                                                                                          ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("        bool finalizeObject = false;                                                                                                                                                                            ");
            funcoes.AppendLine("        void FinalizeObject(bool _disposing)                                                                                                                                                                    ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            if (this.finalizeObject)                                                                                                                                                                            ");
            funcoes.AppendLine("                return;                                                                                                                                                                                         ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("            if (_disposing)                                                                                                                                                                                     ");
            funcoes.AppendLine("                DisposeObjects();                                                                                                                                                                               ");
            funcoes.AppendLine("                                                                                                                                                                                                                ");
            funcoes.AppendLine("            finalizeObject = true;                                                                                                                                                                              ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("        protected virtual void DisposeObjects()                                                                                                                                                                 ");
            funcoes.AppendLine("        {                                                                                                                                                                                                       ");
            funcoes.AppendLine("            propertiesCache = null;                                                                                                                                                                             ");
            funcoes.AppendLine("            eventArgumentCache = null;                                                                                                                                                                          ");
            funcoes.AppendLine("        }                                                                                                                                                                                                       ");
            funcoes.AppendLine("    }                                                                                                                                                                                                           ");
            funcoes.AppendLine("}                                                                                                                                                                                                               ");
            return funcoes.ToString();
        }

    }
}
