using CoreLogic.PluginBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Weikio.PluginFramework.Abstractions;
using Weikio.PluginFramework.Catalogs;
using Weikio.PluginFramework.Catalogs.Delegates;

namespace DBStudioLite.Plugins
{
    /*
     * AppendTargetFrameworkToOutputPath controls framework name 
     * AppendRuntimeIdentifierToOutputPath controls x64/x86
     * https://stackoverflow.com/questions/58757877/new-microsoft-net-sdk-projects-build-output-files-in-subdirectories-how-to-chan
     */
    internal class PluginHandler
    {
        private static CompositePluginCatalog _allPluginsCatalog = new CompositePluginCatalog();
        private static List<Plugin> _plugins;
        private static bool _isPluginsEnabled = false;

        public static async Task<bool> CreateCatalogs()
        {
            _isPluginsEnabled = (ConfigurationManager.AppSettings["IsPluginsEnabled"]??"").ToLower() == "true";
            if (!_isPluginsEnabled) return true;
            // Folder catalog is used to add DB DAL implementations 
            // Note: The plugins must be manually built as it isn't referenced by this Application
            var folderCatalog = new FolderPluginCatalog(
                ConfigurationManager.AppSettings["PluginsFolder"],
                type =>
                {
                    type.Implements<IDynamicDAL>()
                        .Tag("dynamicDAL");
                });

            _allPluginsCatalog.AddCatalog(folderCatalog);

            // Finally the plugin catalog is initialized
            await _allPluginsCatalog.Initialize();
            _plugins = _allPluginsCatalog.GetPlugins();
            return _plugins.Count() > 0;
        }
        public static IDynamicDAL GetDynamicDAL(string sDBIdentifier, string sConnectionString)
        {
            if (!_isPluginsEnabled)
            {
                switch (sDBIdentifier)
                {
                    case "SQLite":
                        return new DynamicDALSQLite(sConnectionString);
                    default:
                        return new DynamicDALSqlServer(sConnectionString);
                }

            }

            var pluginType = _plugins.FirstOrDefault(plugin => plugin.Name.Contains(sDBIdentifier));
            if (pluginType == null)
            {
                throw new Exception($"{sDBIdentifier} type plugin is not available.");
            }
            var pluginInstance = (IDynamicDAL)Activator.CreateInstance(pluginType, sConnectionString);

            return pluginInstance;
        }
    }
}
