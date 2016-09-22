using log4net;
using System;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace Utils
{
    public class Log4NetWrap
    {
        private ILog _log;

        public Log4NetWrap(string name)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(name);
        }

        // Summary:
        //     Log a message object with the log4net.Core.Level.Debug level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //      This method first checks if this logger is DEBUG enabled by comparing the
        //     level of this logger with the log4net.Core.Level.Debug level. If this logger
        //     is DEBUG enabled, then it converts the message object (passed as parameter)
        //     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        //     It then proceeds to call all the registered appenders in this logger and
        //     also higher in the hierarchy depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the
        //     name of the System.Exception but no stack trace. To print a stack trace use
        //     the Debug(object,Exception) form instead.
        public void Debug(object message)
        {
            _log.Debug(message);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Debug level including the
        //     stack trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //      See the Debug(object) form for more detailed information.
        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }
        //
        // Summary:
        //     Logs a formatted message string with the log4net.Core.Level.Debug level.
        //
        // Parameters:
        //   format:
        //     A String containing zero or more format items
        //
        //   args:
        //     An Object array containing zero or more objects to format
        //
        // Remarks:
        //      The message is formatted using the String.Format method. See String.Format(string,
        //     object[]) for details of the syntax of the format string and the behavior
        //     of the formatting.
        //     This method does not take an System.Exception object to include in the log
        //     event. To pass an System.Exception use one of the Debug(object,Exception)
        //     methods instead.
        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }
        //
        // Summary:
        //     Logs a message object with the log4net.Core.Level.Error level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //      This method first checks if this logger is ERROR enabled by comparing the
        //     level of this logger with the log4net.Core.Level.Error level. If this logger
        //     is ERROR enabled, then it converts the message object (passed as parameter)
        //     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        //     It then proceeds to call all the registered appenders in this logger and
        //     also higher in the hierarchy depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the
        //     name of the System.Exception but no stack trace. To print a stack trace use
        //     the Error(object,Exception) form instead.
        public void Error(object message)
        {
            _log.Error(message);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Error level including the
        //     stack trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //      See the Error(object) form for more detailed information.
        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }
        //
        // Summary:
        //     Logs a formatted message string with the log4net.Core.Level.Error level.
        //
        // Parameters:
        //   format:
        //     A String containing zero or more format items
        //
        //   args:
        //     An Object array containing zero or more objects to format
        //
        // Remarks:
        //      The message is formatted using the String.Format method. See String.Format(string,
        //     object[]) for details of the syntax of the format string and the behavior
        //     of the formatting.
        //     This method does not take an System.Exception object to include in the log
        //     event. To pass an System.Exception use one of the Error(object) methods instead.
        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Fatal level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //      This method first checks if this logger is FATAL enabled by comparing the
        //     level of this logger with the log4net.Core.Level.Fatal level. If this logger
        //     is FATAL enabled, then it converts the message object (passed as parameter)
        //     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        //     It then proceeds to call all the registered appenders in this logger and
        //     also higher in the hierarchy depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the
        //     name of the System.Exception but no stack trace. To print a stack trace use
        //     the Fatal(object,Exception) form instead.
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Fatal level including the
        //     stack trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //      See the Fatal(object) form for more detailed information.
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }
        //
        // Summary:
        //     Logs a formatted message string with the log4net.Core.Level.Fatal level.
        //
        // Parameters:
        //   format:
        //     A String containing zero or more format items
        //
        //   args:
        //     An Object array containing zero or more objects to format
        //
        // Remarks:
        //      The message is formatted using the String.Format method. See String.Format(string,
        //     object[]) for details of the syntax of the format string and the behavior
        //     of the formatting.
        //     This method does not take an System.Exception object to include in the log
        //     event. To pass an System.Exception use one of the Fatal(object) methods instead.
        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }
        //
        // Summary:
        //     Logs a message object with the log4net.Core.Level.Info level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //      This method first checks if this logger is INFO enabled by comparing the
        //     level of this logger with the log4net.Core.Level.Info level. If this logger
        //     is INFO enabled, then it converts the message object (passed as parameter)
        //     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        //     It then proceeds to call all the registered appenders in this logger and
        //     also higher in the hierarchy depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the
        //     name of the System.Exception but no stack trace. To print a stack trace use
        //     the Info(object,Exception) form instead.
        public void Info(object message)
        {
            _log.Info(message);
        }
        //
        // Summary:
        //     Logs a message object with the INFO level including the stack trace of the
        //     System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //      See the Info(object) form for more detailed information.
        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }
        //
        // Summary:
        //     Logs a formatted message string with the log4net.Core.Level.Info level.
        //
        // Parameters:
        //   format:
        //     A String containing zero or more format items
        //
        //   args:
        //     An Object array containing zero or more objects to format
        //
        // Remarks:
        //      The message is formatted using the String.Format method. See String.Format(string,
        //     object[]) for details of the syntax of the format string and the behavior
        //     of the formatting.
        //     This method does not take an System.Exception object to include in the log
        //     event. To pass an System.Exception use one of the Info(object) methods instead.
        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Warn level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //      This method first checks if this logger is WARN enabled by comparing the
        //     level of this logger with the log4net.Core.Level.Warn level. If this logger
        //     is WARN enabled, then it converts the message object (passed as parameter)
        //     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        //     It then proceeds to call all the registered appenders in this logger and
        //     also higher in the hierarchy depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the
        //     name of the System.Exception but no stack trace. To print a stack trace use
        //     the Warn(object,Exception) form instead.
        public void Warn(object message)
        {
            _log.Warn(message);
        }
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Warn level including the
        //     stack trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //      See the Warn(object) form for more detailed information.
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }
        //
        // Summary:
        //     Logs a formatted message string with the log4net.Core.Level.Warn level.
        //
        // Parameters:
        //   format:
        //     A String containing zero or more format items
        //
        //   args:
        //     An Object array containing zero or more objects to format
        //
        // Remarks:
        //      The message is formatted using the String.Format method. See String.Format(string,
        //     object[]) for details of the syntax of the format string and the behavior
        //     of the formatting.
        //     This method does not take an System.Exception object to include in the log
        //     event. To pass an System.Exception use one of the Warn(object) methods instead.
        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
    }
}
