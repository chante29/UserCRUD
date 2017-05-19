using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UserCRUDRest.Utils
{
    public static class Container
    {
        #region Constants

        private const string clonedUnityConfigFileName = "clonedUnityConf.config";
        #endregion

        #region Properties

        static IUnityContainer currentContainer;

        private static IUnityContainer Current
        {
            get
            {
                return currentContainer;
            }
        }

        #endregion

        #region Constructor

        static Container()
        {
            ConfigureContainer();
        }

        #endregion

        #region Methods

        static void ConfigureContainer()
        {
            FileIoManager fileManager = new FileIoManager();
            fileManager.CurrentAssembly = Assembly.GetExecutingAssembly();

            //var unityEmbeddedResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Unity.config");
            string unityEmbeddedResourcePath = "UserCRUDRest.unity.config";
            string unityConfigContent = fileManager.ReadEmbeddedResource(unityEmbeddedResourcePath);
            fileManager.WriteTextFileToAssemblyDirectory(string.Empty, clonedUnityConfigFileName, unityConfigContent);

            string temporalUnityConfigPath = fileManager.GetFullPathFromFileInAssemblyDirectory(string.Empty, clonedUnityConfigFileName);

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = temporalUnityConfigPath };

            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                                                ConfigurationUserLevel.None);

            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

            currentContainer = new UnityContainer();
            currentContainer.LoadConfiguration(unitySection);
        }

        public static T Resolve<T>()
        {
            return Resolve<T>(string.Empty);
        }

        private static T Resolve<T>(string name)
        {
            return (T)Current.Resolve(typeof(T), name);
        }

        #endregion
    }
}