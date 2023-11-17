///  
/// The :mod:`~uo.utils.files` module contains utility functions that deals with files.
/// 
namespace utils {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using os;
    
    public static class files {
        
        public static object directory = Path(_file__).resolve();
        
        static files() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        /// 
        /// Ensure existence of the specific directory in the file system
        /// 
        /// :param path_to_dir:str -- path of the directory whose existence should be ensured
        /// 
        public static object ensure_dir(string path_to_dir) {
            if (!os.path.exists(path_to_dir)) {
                os.mkdir(path_to_dir);
            }
        }
    }
}
