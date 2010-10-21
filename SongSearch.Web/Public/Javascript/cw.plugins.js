// Name:        MicrosoftAjax.debug.js
// Assembly:    System.Web.Extensions
// Version:     4.0.0.0
// FileVersion: 4.0.20526.0
//-----------------------------------------------------------------------
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------
// MicrosoftAjax.js
// Microsoft AJAX Framework.
 
Function.__typeName = 'Function';
Function.__class = true;
Function.createCallback = function Function$createCallback(method, context) {
    /// <summary locid="M:J#Function.createCallback" />
    /// <param name="method" type="Function"></param>
    /// <param name="context" mayBeNull="true"></param>
    /// <returns type="Function"></returns>
    var e = Function._validateParams(arguments, [
        {name: "method", type: Function},
        {name: "context", mayBeNull: true}
    ]);
    if (e) throw e;
    return function() {
        var l = arguments.length;
        if (l > 0) {
            var args = [];
            for (var i = 0; i < l; i++) {
                args[i] = arguments[i];
            }
            args[l] = context;
            return method.apply(this, args);
        }
        return method.call(this, context);
    }
}
Function.createDelegate = function Function$createDelegate(instance, method) {
    /// <summary locid="M:J#Function.createDelegate" />
    /// <param name="instance" mayBeNull="true"></param>
    /// <param name="method" type="Function"></param>
    /// <returns type="Function"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", mayBeNull: true},
        {name: "method", type: Function}
    ]);
    if (e) throw e;
    return function() {
        return method.apply(instance, arguments);
    }
}
Function.emptyFunction = Function.emptyMethod = function Function$emptyMethod() {
    /// <summary locid="M:J#Function.emptyMethod" />
}
Function.validateParameters = function Function$validateParameters(parameters, expectedParameters, validateParameterCount) {
    /// <summary locid="M:J#Function.validateParameters" />
    /// <param name="parameters"></param>
    /// <param name="expectedParameters"></param>
    /// <param name="validateParameterCount" type="Boolean" optional="true"></param>
    /// <returns type="Error" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "parameters"},
        {name: "expectedParameters"},
        {name: "validateParameterCount", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    return Function._validateParams(parameters, expectedParameters, validateParameterCount);
}
Function._validateParams = function Function$_validateParams(params, expectedParams, validateParameterCount) {
    var e, expectedLength = expectedParams.length;
    validateParameterCount = validateParameterCount || (typeof(validateParameterCount) === "undefined");
    e = Function._validateParameterCount(params, expectedParams, validateParameterCount);
    if (e) {
        e.popStackFrame();
        return e;
    }
    for (var i = 0, l = params.length; i < l; i++) {
        var expectedParam = expectedParams[Math.min(i, expectedLength - 1)],
            paramName = expectedParam.name;
        if (expectedParam.parameterArray) {
            paramName += "[" + (i - expectedLength + 1) + "]";
        }
        else if (!validateParameterCount && (i >= expectedLength)) {
            break;
        }
        e = Function._validateParameter(params[i], expectedParam, paramName);
        if (e) {
            e.popStackFrame();
            return e;
        }
    }
    return null;
}
Function._validateParameterCount = function Function$_validateParameterCount(params, expectedParams, validateParameterCount) {
    var i, error,
        expectedLen = expectedParams.length,
        actualLen = params.length;
    if (actualLen < expectedLen) {
        var minParams = expectedLen;
        for (i = 0; i < expectedLen; i++) {
            var param = expectedParams[i];
            if (param.optional || param.parameterArray) {
                minParams--;
            }
        }        
        if (actualLen < minParams) {
            error = true;
        }
    }
    else if (validateParameterCount && (actualLen > expectedLen)) {
        error = true;      
        for (i = 0; i < expectedLen; i++) {
            if (expectedParams[i].parameterArray) {
                error = false; 
                break;
            }
        }  
    }
    if (error) {
        var e = Error.parameterCount();
        e.popStackFrame();
        return e;
    }
    return null;
}
Function._validateParameter = function Function$_validateParameter(param, expectedParam, paramName) {
    var e,
        expectedType = expectedParam.type,
        expectedInteger = !!expectedParam.integer,
        expectedDomElement = !!expectedParam.domElement,
        mayBeNull = !!expectedParam.mayBeNull;
    e = Function._validateParameterType(param, expectedType, expectedInteger, expectedDomElement, mayBeNull, paramName);
    if (e) {
        e.popStackFrame();
        return e;
    }
    var expectedElementType = expectedParam.elementType,
        elementMayBeNull = !!expectedParam.elementMayBeNull;
    if (expectedType === Array && typeof(param) !== "undefined" && param !== null &&
        (expectedElementType || !elementMayBeNull)) {
        var expectedElementInteger = !!expectedParam.elementInteger,
            expectedElementDomElement = !!expectedParam.elementDomElement;
        for (var i=0; i < param.length; i++) {
            var elem = param[i];
            e = Function._validateParameterType(elem, expectedElementType,
                expectedElementInteger, expectedElementDomElement, elementMayBeNull,
                paramName + "[" + i + "]");
            if (e) {
                e.popStackFrame();
                return e;
            }
        }
    }
    return null;
}
Function._validateParameterType = function Function$_validateParameterType(param, expectedType, expectedInteger, expectedDomElement, mayBeNull, paramName) {
    var e, i;
    if (typeof(param) === "undefined") {
        if (mayBeNull) {
            return null;
        }
        else {
            e = Error.argumentUndefined(paramName);
            e.popStackFrame();
            return e;
        }
    }
    if (param === null) {
        if (mayBeNull) {
            return null;
        }
        else {
            e = Error.argumentNull(paramName);
            e.popStackFrame();
            return e;
        }
    }
    if (expectedType && expectedType.__enum) {
        if (typeof(param) !== 'number') {
            e = Error.argumentType(paramName, Object.getType(param), expectedType);
            e.popStackFrame();
            return e;
        }
        if ((param % 1) === 0) {
            var values = expectedType.prototype;
            if (!expectedType.__flags || (param === 0)) {
                for (i in values) {
                    if (values[i] === param) return null;
                }
            }
            else {
                var v = param;
                for (i in values) {
                    var vali = values[i];
                    if (vali === 0) continue;
                    if ((vali & param) === vali) {
                        v -= vali;
                    }
                    if (v === 0) return null;
                }
            }
        }
        e = Error.argumentOutOfRange(paramName, param, String.format(Sys.Res.enumInvalidValue, param, expectedType.getName()));
        e.popStackFrame();
        return e;
    }
    if (expectedDomElement && (!Sys._isDomElement(param) || (param.nodeType === 3))) {
        e = Error.argument(paramName, Sys.Res.argumentDomElement);
        e.popStackFrame();
        return e;
    }
    if (expectedType && !Sys._isInstanceOfType(expectedType, param)) {
        e = Error.argumentType(paramName, Object.getType(param), expectedType);
        e.popStackFrame();
        return e;
    }
    if (expectedType === Number && expectedInteger) {
        if ((param % 1) !== 0) {
            e = Error.argumentOutOfRange(paramName, param, Sys.Res.argumentInteger);
            e.popStackFrame();
            return e;
        }
    }
    return null;
}
 
Error.__typeName = 'Error';
Error.__class = true;
Error.create = function Error$create(message, errorInfo) {
    /// <summary locid="M:J#Error.create" />
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="errorInfo" optional="true" mayBeNull="true"></param>
    /// <returns type="Error"></returns>
    var e = Function._validateParams(arguments, [
        {name: "message", type: String, mayBeNull: true, optional: true},
        {name: "errorInfo", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var err = new Error(message);
    err.message = message;
    if (errorInfo) {
        for (var v in errorInfo) {
            err[v] = errorInfo[v];
        }
    }
    err.popStackFrame();
    return err;
}
Error.argument = function Error$argument(paramName, message) {
    /// <summary locid="M:J#Error.argument" />
    /// <param name="paramName" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "paramName", type: String, mayBeNull: true, optional: true},
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ArgumentException: " + (message ? message : Sys.Res.argument);
    if (paramName) {
        displayMessage += "\n" + String.format(Sys.Res.paramName, paramName);
    }
    var err = Error.create(displayMessage, { name: "Sys.ArgumentException", paramName: paramName });
    err.popStackFrame();
    return err;
}
Error.argumentNull = function Error$argumentNull(paramName, message) {
    /// <summary locid="M:J#Error.argumentNull" />
    /// <param name="paramName" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "paramName", type: String, mayBeNull: true, optional: true},
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ArgumentNullException: " + (message ? message : Sys.Res.argumentNull);
    if (paramName) {
        displayMessage += "\n" + String.format(Sys.Res.paramName, paramName);
    }
    var err = Error.create(displayMessage, { name: "Sys.ArgumentNullException", paramName: paramName });
    err.popStackFrame();
    return err;
}
Error.argumentOutOfRange = function Error$argumentOutOfRange(paramName, actualValue, message) {
    /// <summary locid="M:J#Error.argumentOutOfRange" />
    /// <param name="paramName" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="actualValue" optional="true" mayBeNull="true"></param>
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "paramName", type: String, mayBeNull: true, optional: true},
        {name: "actualValue", mayBeNull: true, optional: true},
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ArgumentOutOfRangeException: " + (message ? message : Sys.Res.argumentOutOfRange);
    if (paramName) {
        displayMessage += "\n" + String.format(Sys.Res.paramName, paramName);
    }
    if (typeof(actualValue) !== "undefined" && actualValue !== null) {
        displayMessage += "\n" + String.format(Sys.Res.actualValue, actualValue);
    }
    var err = Error.create(displayMessage, {
        name: "Sys.ArgumentOutOfRangeException",
        paramName: paramName,
        actualValue: actualValue
    });
    err.popStackFrame();
    return err;
}
Error.argumentType = function Error$argumentType(paramName, actualType, expectedType, message) {
    /// <summary locid="M:J#Error.argumentType" />
    /// <param name="paramName" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="actualType" type="Type" optional="true" mayBeNull="true"></param>
    /// <param name="expectedType" type="Type" optional="true" mayBeNull="true"></param>
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "paramName", type: String, mayBeNull: true, optional: true},
        {name: "actualType", type: Type, mayBeNull: true, optional: true},
        {name: "expectedType", type: Type, mayBeNull: true, optional: true},
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ArgumentTypeException: ";
    if (message) {
        displayMessage += message;
    }
    else if (actualType && expectedType) {
        displayMessage +=
            String.format(Sys.Res.argumentTypeWithTypes, actualType.getName(), expectedType.getName());
    }
    else {
        displayMessage += Sys.Res.argumentType;
    }
    if (paramName) {
        displayMessage += "\n" + String.format(Sys.Res.paramName, paramName);
    }
    var err = Error.create(displayMessage, {
        name: "Sys.ArgumentTypeException",
        paramName: paramName,
        actualType: actualType,
        expectedType: expectedType
    });
    err.popStackFrame();
    return err;
}
Error.argumentUndefined = function Error$argumentUndefined(paramName, message) {
    /// <summary locid="M:J#Error.argumentUndefined" />
    /// <param name="paramName" type="String" optional="true" mayBeNull="true"></param>
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "paramName", type: String, mayBeNull: true, optional: true},
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ArgumentUndefinedException: " + (message ? message : Sys.Res.argumentUndefined);
    if (paramName) {
        displayMessage += "\n" + String.format(Sys.Res.paramName, paramName);
    }
    var err = Error.create(displayMessage, { name: "Sys.ArgumentUndefinedException", paramName: paramName });
    err.popStackFrame();
    return err;
}
Error.format = function Error$format(message) {
    /// <summary locid="M:J#Error.format" />
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.FormatException: " + (message ? message : Sys.Res.format);
    var err = Error.create(displayMessage, {name: 'Sys.FormatException'});
    err.popStackFrame();
    return err;
}
Error.invalidOperation = function Error$invalidOperation(message) {
    /// <summary locid="M:J#Error.invalidOperation" />
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.InvalidOperationException: " + (message ? message : Sys.Res.invalidOperation);
    var err = Error.create(displayMessage, {name: 'Sys.InvalidOperationException'});
    err.popStackFrame();
    return err;
}
Error.notImplemented = function Error$notImplemented(message) {
    /// <summary locid="M:J#Error.notImplemented" />
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.NotImplementedException: " + (message ? message : Sys.Res.notImplemented);
    var err = Error.create(displayMessage, {name: 'Sys.NotImplementedException'});
    err.popStackFrame();
    return err;
}
Error.parameterCount = function Error$parameterCount(message) {
    /// <summary locid="M:J#Error.parameterCount" />
    /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "message", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var displayMessage = "Sys.ParameterCountException: " + (message ? message : Sys.Res.parameterCount);
    var err = Error.create(displayMessage, {name: 'Sys.ParameterCountException'});
    err.popStackFrame();
    return err;
}
Error.prototype.popStackFrame = function Error$popStackFrame() {
    /// <summary locid="M:J#checkParam" />
    if (arguments.length !== 0) throw Error.parameterCount();
    if (typeof(this.stack) === "undefined" || this.stack === null ||
        typeof(this.fileName) === "undefined" || this.fileName === null ||
        typeof(this.lineNumber) === "undefined" || this.lineNumber === null) {
        return;
    }
    var stackFrames = this.stack.split("\n");
    var currentFrame = stackFrames[0];
    var pattern = this.fileName + ":" + this.lineNumber;
    while(typeof(currentFrame) !== "undefined" &&
          currentFrame !== null &&
          currentFrame.indexOf(pattern) === -1) {
        stackFrames.shift();
        currentFrame = stackFrames[0];
    }
    var nextFrame = stackFrames[1];
    if (typeof(nextFrame) === "undefined" || nextFrame === null) {
        return;
    }
    var nextFrameParts = nextFrame.match(/@(.*):(\d+)$/);
    if (typeof(nextFrameParts) === "undefined" || nextFrameParts === null) {
        return;
    }
    this.fileName = nextFrameParts[1];
    this.lineNumber = parseInt(nextFrameParts[2]);
    stackFrames.shift();
    this.stack = stackFrames.join("\n");
}
 
Object.__typeName = 'Object';
Object.__class = true;
Object.getType = function Object$getType(instance) {
    /// <summary locid="M:J#Object.getType" />
    /// <param name="instance"></param>
    /// <returns type="Type"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance"}
    ]);
    if (e) throw e;
    var ctor = instance.constructor;
    if (!ctor || (typeof(ctor) !== "function") || !ctor.__typeName || (ctor.__typeName === 'Object')) {
        return Object;
    }
    return ctor;
}
Object.getTypeName = function Object$getTypeName(instance) {
    /// <summary locid="M:J#Object.getTypeName" />
    /// <param name="instance"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance"}
    ]);
    if (e) throw e;
    return Object.getType(instance).getName();
}
 
String.__typeName = 'String';
String.__class = true;
String.prototype.endsWith = function String$endsWith(suffix) {
    /// <summary locid="M:J#String.endsWith" />
    /// <param name="suffix" type="String"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "suffix", type: String}
    ]);
    if (e) throw e;
    return (this.substr(this.length - suffix.length) === suffix);
}
String.prototype.startsWith = function String$startsWith(prefix) {
    /// <summary locid="M:J#String.startsWith" />
    /// <param name="prefix" type="String"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "prefix", type: String}
    ]);
    if (e) throw e;
    return (this.substr(0, prefix.length) === prefix);
}
String.prototype.trim = function String$trim() {
    /// <summary locid="M:J#String.trim" />
    /// <returns type="String"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return this.replace(/^\s+|\s+$/g, '');
}
String.prototype.trimEnd = function String$trimEnd() {
    /// <summary locid="M:J#String.trimEnd" />
    /// <returns type="String"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return this.replace(/\s+$/, '');
}
String.prototype.trimStart = function String$trimStart() {
    /// <summary locid="M:J#String.trimStart" />
    /// <returns type="String"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return this.replace(/^\s+/, '');
}
String.format = function String$format(format, args) {
    /// <summary locid="M:J#String.format" />
    /// <param name="format" type="String"></param>
    /// <param name="args" parameterArray="true" mayBeNull="true"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String},
        {name: "args", mayBeNull: true, parameterArray: true}
    ]);
    if (e) throw e;
    return String._toFormattedString(false, arguments);
}
String._toFormattedString = function String$_toFormattedString(useLocale, args) {
    var result = '';
    var format = args[0];
    for (var i=0;;) {
        var open = format.indexOf('{', i);
        var close = format.indexOf('}', i);
        if ((open < 0) && (close < 0)) {
            result += format.slice(i);
            break;
        }
        if ((close > 0) && ((close < open) || (open < 0))) {
            if (format.charAt(close + 1) !== '}') {
                throw Error.argument('format', Sys.Res.stringFormatBraceMismatch);
            }
            result += format.slice(i, close + 1);
            i = close + 2;
            continue;
        }
        result += format.slice(i, open);
        i = open + 1;
        if (format.charAt(i) === '{') {
            result += '{';
            i++;
            continue;
        }
        if (close < 0) throw Error.argument('format', Sys.Res.stringFormatBraceMismatch);
        var brace = format.substring(i, close);
        var colonIndex = brace.indexOf(':');
        var argNumber = parseInt((colonIndex < 0)? brace : brace.substring(0, colonIndex), 10) + 1;
        if (isNaN(argNumber)) throw Error.argument('format', Sys.Res.stringFormatInvalid);
        var argFormat = (colonIndex < 0)? '' : brace.substring(colonIndex + 1);
        var arg = args[argNumber];
        if (typeof(arg) === "undefined" || arg === null) {
            arg = '';
        }
        if (arg.toFormattedString) {
            result += arg.toFormattedString(argFormat);
        }
        else if (useLocale && arg.localeFormat) {
            result += arg.localeFormat(argFormat);
        }
        else if (arg.format) {
            result += arg.format(argFormat);
        }
        else
            result += arg.toString();
        i = close + 1;
    }
    return result;
}
 
Boolean.__typeName = 'Boolean';
Boolean.__class = true;
Boolean.parse = function Boolean$parse(value) {
    /// <summary locid="M:J#Boolean.parse" />
    /// <param name="value" type="String"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String}
    ], false);
    if (e) throw e;
    var v = value.trim().toLowerCase();
    if (v === 'false') return false;
    if (v === 'true') return true;
    throw Error.argumentOutOfRange('value', value, Sys.Res.boolTrueOrFalse);
}
 
Date.__typeName = 'Date';
Date.__class = true;
 
Number.__typeName = 'Number';
Number.__class = true;
 
RegExp.__typeName = 'RegExp';
RegExp.__class = true;
 
if (!window) this.window = this;
window.Type = Function;
Type.__fullyQualifiedIdentifierRegExp = new RegExp("^[^.0-9 \\s|,;:&*=+\\-()\\[\\]{}^%#@!~\\n\\r\\t\\f\\\\]([^ \\s|,;:&*=+\\-()\\[\\]{}^%#@!~\\n\\r\\t\\f\\\\]*[^. \\s|,;:&*=+\\-()\\[\\]{}^%#@!~\\n\\r\\t\\f\\\\])?$", "i");
Type.__identifierRegExp = new RegExp("^[^.0-9 \\s|,;:&*=+\\-()\\[\\]{}^%#@!~\\n\\r\\t\\f\\\\][^. \\s|,;:&*=+\\-()\\[\\]{}^%#@!~\\n\\r\\t\\f\\\\]*$", "i");
Type.prototype.callBaseMethod = function Type$callBaseMethod(instance, name, baseArguments) {
    /// <summary locid="M:J#Type.callBaseMethod" />
    /// <param name="instance"></param>
    /// <param name="name" type="String"></param>
    /// <param name="baseArguments" type="Array" optional="true" mayBeNull="true" elementMayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance"},
        {name: "name", type: String},
        {name: "baseArguments", type: Array, mayBeNull: true, optional: true, elementMayBeNull: true}
    ]);
    if (e) throw e;
    var baseMethod = Sys._getBaseMethod(this, instance, name);
    if (!baseMethod) throw Error.invalidOperation(String.format(Sys.Res.methodNotFound, name));
    if (!baseArguments) {
        return baseMethod.apply(instance);
    }
    else {
        return baseMethod.apply(instance, baseArguments);
    }
}
Type.prototype.getBaseMethod = function Type$getBaseMethod(instance, name) {
    /// <summary locid="M:J#Type.getBaseMethod" />
    /// <param name="instance"></param>
    /// <param name="name" type="String"></param>
    /// <returns type="Function" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance"},
        {name: "name", type: String}
    ]);
    if (e) throw e;
    return Sys._getBaseMethod(this, instance, name);
}
Type.prototype.getBaseType = function Type$getBaseType() {
    /// <summary locid="M:J#Type.getBaseType" />
    /// <returns type="Type" mayBeNull="true"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return (typeof(this.__baseType) === "undefined") ? null : this.__baseType;
}
Type.prototype.getInterfaces = function Type$getInterfaces() {
    /// <summary locid="M:J#Type.getInterfaces" />
    /// <returns type="Array" elementType="Type" mayBeNull="false" elementMayBeNull="false"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    var result = [];
    var type = this;
    while(type) {
        var interfaces = type.__interfaces;
        if (interfaces) {
            for (var i = 0, l = interfaces.length; i < l; i++) {
                var interfaceType = interfaces[i];
                if (!Array.contains(result, interfaceType)) {
                    result[result.length] = interfaceType;
                }
            }
        }
        type = type.__baseType;
    }
    return result;
}
Type.prototype.getName = function Type$getName() {
    /// <summary locid="M:J#Type.getName" />
    /// <returns type="String"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return (typeof(this.__typeName) === "undefined") ? "" : this.__typeName;
}
Type.prototype.implementsInterface = function Type$implementsInterface(interfaceType) {
    /// <summary locid="M:J#Type.implementsInterface" />
    /// <param name="interfaceType" type="Type"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "interfaceType", type: Type}
    ]);
    if (e) throw e;
    this.resolveInheritance();
    var interfaceName = interfaceType.getName();
    var cache = this.__interfaceCache;
    if (cache) {
        var cacheEntry = cache[interfaceName];
        if (typeof(cacheEntry) !== 'undefined') return cacheEntry;
    }
    else {
        cache = this.__interfaceCache = {};
    }
    var baseType = this;
    while (baseType) {
        var interfaces = baseType.__interfaces;
        if (interfaces) {
            if (Array.indexOf(interfaces, interfaceType) !== -1) {
                return cache[interfaceName] = true;
            }
        }
        baseType = baseType.__baseType;
    }
    return cache[interfaceName] = false;
}
Type.prototype.inheritsFrom = function Type$inheritsFrom(parentType) {
    /// <summary locid="M:J#Type.inheritsFrom" />
    /// <param name="parentType" type="Type"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "parentType", type: Type}
    ]);
    if (e) throw e;
    this.resolveInheritance();
    var baseType = this.__baseType;
    while (baseType) {
        if (baseType === parentType) {
            return true;
        }
        baseType = baseType.__baseType;
    }
    return false;
}
Type.prototype.initializeBase = function Type$initializeBase(instance, baseArguments) {
    /// <summary locid="M:J#Type.initializeBase" />
    /// <param name="instance"></param>
    /// <param name="baseArguments" type="Array" optional="true" mayBeNull="true" elementMayBeNull="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance"},
        {name: "baseArguments", type: Array, mayBeNull: true, optional: true, elementMayBeNull: true}
    ]);
    if (e) throw e;
    if (!Sys._isInstanceOfType(this, instance)) throw Error.argumentType('instance', Object.getType(instance), this);
    this.resolveInheritance();
    if (this.__baseType) {
        if (!baseArguments) {
            this.__baseType.apply(instance);
        }
        else {
            this.__baseType.apply(instance, baseArguments);
        }
    }
    return instance;
}
Type.prototype.isImplementedBy = function Type$isImplementedBy(instance) {
    /// <summary locid="M:J#Type.isImplementedBy" />
    /// <param name="instance" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", mayBeNull: true}
    ]);
    if (e) throw e;
    if (typeof(instance) === "undefined" || instance === null) return false;
    var instanceType = Object.getType(instance);
    return !!(instanceType.implementsInterface && instanceType.implementsInterface(this));
}
Type.prototype.isInstanceOfType = function Type$isInstanceOfType(instance) {
    /// <summary locid="M:J#Type.isInstanceOfType" />
    /// <param name="instance" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", mayBeNull: true}
    ]);
    if (e) throw e;
    return Sys._isInstanceOfType(this, instance);
}
Type.prototype.registerClass = function Type$registerClass(typeName, baseType, interfaceTypes) {
    /// <summary locid="M:J#Type.registerClass" />
    /// <param name="typeName" type="String"></param>
    /// <param name="baseType" type="Type" optional="true" mayBeNull="true"></param>
    /// <param name="interfaceTypes" parameterArray="true" type="Type"></param>
    /// <returns type="Type"></returns>
    var e = Function._validateParams(arguments, [
        {name: "typeName", type: String},
        {name: "baseType", type: Type, mayBeNull: true, optional: true},
        {name: "interfaceTypes", type: Type, parameterArray: true}
    ]);
    if (e) throw e;
    if (!Type.__fullyQualifiedIdentifierRegExp.test(typeName)) throw Error.argument('typeName', Sys.Res.notATypeName);
    var parsedName;
    try {
        parsedName = eval(typeName);
    }
    catch(e) {
        throw Error.argument('typeName', Sys.Res.argumentTypeName);
    }
    if (parsedName !== this) throw Error.argument('typeName', Sys.Res.badTypeName);
    if (Sys.__registeredTypes[typeName]) throw Error.invalidOperation(String.format(Sys.Res.typeRegisteredTwice, typeName));
    if ((arguments.length > 1) && (typeof(baseType) === 'undefined')) throw Error.argumentUndefined('baseType');
    if (baseType && !baseType.__class) throw Error.argument('baseType', Sys.Res.baseNotAClass);
    this.prototype.constructor = this;
    this.__typeName = typeName;
    this.__class = true;
    if (baseType) {
        this.__baseType = baseType;
        this.__basePrototypePending = true;
    }
    Sys.__upperCaseTypes[typeName.toUpperCase()] = this;
    if (interfaceTypes) {
        this.__interfaces = [];
        this.resolveInheritance();
        for (var i = 2, l = arguments.length; i < l; i++) {
            var interfaceType = arguments[i];
            if (!interfaceType.__interface) throw Error.argument('interfaceTypes[' + (i - 2) + ']', Sys.Res.notAnInterface);
            for (var methodName in interfaceType.prototype) {
                var method = interfaceType.prototype[methodName];
                if (!this.prototype[methodName]) {
                    this.prototype[methodName] = method;
                }
            }
            this.__interfaces.push(interfaceType);
        }
    }
    Sys.__registeredTypes[typeName] = true;
    return this;
}
Type.prototype.registerInterface = function Type$registerInterface(typeName) {
    /// <summary locid="M:J#Type.registerInterface" />
    /// <param name="typeName" type="String"></param>
    /// <returns type="Type"></returns>
    var e = Function._validateParams(arguments, [
        {name: "typeName", type: String}
    ]);
    if (e) throw e;
    if (!Type.__fullyQualifiedIdentifierRegExp.test(typeName)) throw Error.argument('typeName', Sys.Res.notATypeName);
    var parsedName;
    try {
        parsedName = eval(typeName);
    }
    catch(e) {
        throw Error.argument('typeName', Sys.Res.argumentTypeName);
    }
    if (parsedName !== this) throw Error.argument('typeName', Sys.Res.badTypeName);
    if (Sys.__registeredTypes[typeName]) throw Error.invalidOperation(String.format(Sys.Res.typeRegisteredTwice, typeName));
    Sys.__upperCaseTypes[typeName.toUpperCase()] = this;
    this.prototype.constructor = this;
    this.__typeName = typeName;
    this.__interface = true;
    Sys.__registeredTypes[typeName] = true;
    return this;
}
Type.prototype.resolveInheritance = function Type$resolveInheritance() {
    /// <summary locid="M:J#Type.resolveInheritance" />
    if (arguments.length !== 0) throw Error.parameterCount();
    if (this.__basePrototypePending) {
        var baseType = this.__baseType;
        baseType.resolveInheritance();
        for (var memberName in baseType.prototype) {
            var memberValue = baseType.prototype[memberName];
            if (!this.prototype[memberName]) {
                this.prototype[memberName] = memberValue;
            }
        }
        delete this.__basePrototypePending;
    }
}
Type.getRootNamespaces = function Type$getRootNamespaces() {
    /// <summary locid="M:J#Type.getRootNamespaces" />
    /// <returns type="Array"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return Array.clone(Sys.__rootNamespaces);
}
Type.isClass = function Type$isClass(type) {
    /// <summary locid="M:J#Type.isClass" />
    /// <param name="type" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", mayBeNull: true}
    ]);
    if (e) throw e;
    if ((typeof(type) === 'undefined') || (type === null)) return false;
    return !!type.__class;
}
Type.isInterface = function Type$isInterface(type) {
    /// <summary locid="M:J#Type.isInterface" />
    /// <param name="type" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", mayBeNull: true}
    ]);
    if (e) throw e;
    if ((typeof(type) === 'undefined') || (type === null)) return false;
    return !!type.__interface;
}
Type.isNamespace = function Type$isNamespace(object) {
    /// <summary locid="M:J#Type.isNamespace" />
    /// <param name="object" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "object", mayBeNull: true}
    ]);
    if (e) throw e;
    if ((typeof(object) === 'undefined') || (object === null)) return false;
    return !!object.__namespace;
}
Type.parse = function Type$parse(typeName, ns) {
    /// <summary locid="M:J#Type.parse" />
    /// <param name="typeName" type="String" mayBeNull="true"></param>
    /// <param name="ns" optional="true" mayBeNull="true"></param>
    /// <returns type="Type" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "typeName", type: String, mayBeNull: true},
        {name: "ns", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var fn;
    if (ns) {
        fn = Sys.__upperCaseTypes[ns.getName().toUpperCase() + '.' + typeName.toUpperCase()];
        return fn || null;
    }
    if (!typeName) return null;
    if (!Type.__htClasses) {
        Type.__htClasses = {};
    }
    fn = Type.__htClasses[typeName];
    if (!fn) {
        fn = eval(typeName);
        if (typeof(fn) !== 'function') throw Error.argument('typeName', Sys.Res.notATypeName);
        Type.__htClasses[typeName] = fn;
    }
    return fn;
}
Type.registerNamespace = function Type$registerNamespace(namespacePath) {
    /// <summary locid="M:J#Type.registerNamespace" />
    /// <param name="namespacePath" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "namespacePath", type: String}
    ]);
    if (e) throw e;
    Type._registerNamespace(namespacePath);
}
Type._registerNamespace = function Type$_registerNamespace(namespacePath) {
    if (!Type.__fullyQualifiedIdentifierRegExp.test(namespacePath)) throw Error.argument('namespacePath', Sys.Res.invalidNameSpace);
    var rootObject = window;
    var namespaceParts = namespacePath.split('.');
    for (var i = 0; i < namespaceParts.length; i++) {
        var currentPart = namespaceParts[i];
        var ns = rootObject[currentPart];
        var nsType = typeof(ns);
        if ((nsType !== "undefined") && (ns !== null)) {
            if (nsType === "function") {
                throw Error.invalidOperation(String.format(Sys.Res.namespaceContainsClass, namespaceParts.splice(0, i + 1).join('.')));
            }
            if ((typeof(ns) !== "object") || (ns instanceof Array)) {
                throw Error.invalidOperation(String.format(Sys.Res.namespaceContainsNonObject, namespaceParts.splice(0, i + 1).join('.')));
            }
        }
        if (!ns) {
            ns = rootObject[currentPart] = {};
        }
        if (!ns.__namespace) {
            if ((i === 0) && (namespacePath !== "Sys")) {
                Sys.__rootNamespaces[Sys.__rootNamespaces.length] = ns;
            }
            ns.__namespace = true;
            ns.__typeName = namespaceParts.slice(0, i + 1).join('.');
            var parsedName;
            try {
                parsedName = eval(ns.__typeName);
            }
            catch(e) {
                parsedName = null;
            }
            if (parsedName !== ns) {
                delete rootObject[currentPart];
                throw Error.argument('namespacePath', Sys.Res.invalidNameSpace);
            }
            ns.getName = function ns$getName() {return this.__typeName;}
        }
        rootObject = ns;
    }
}
Type._checkDependency = function Type$_checkDependency(dependency, featureName) {
    var scripts = Type._registerScript._scripts, isDependent = (scripts ? (!!scripts[dependency]) : false);
    if ((typeof(featureName) !== 'undefined') && !isDependent) {
        throw Error.invalidOperation(String.format(Sys.Res.requiredScriptReferenceNotIncluded, 
        featureName, dependency));
    }
    return isDependent;
}
Type._registerScript = function Type$_registerScript(scriptName, dependencies) {
    var scripts = Type._registerScript._scripts;
    if (!scripts) {
        Type._registerScript._scripts = scripts = {};
    }
    if (scripts[scriptName]) {
        throw Error.invalidOperation(String.format(Sys.Res.scriptAlreadyLoaded, scriptName));
    }
    scripts[scriptName] = true;
    if (dependencies) {
        for (var i = 0, l = dependencies.length; i < l; i++) {
            var dependency = dependencies[i];
            if (!Type._checkDependency(dependency)) {
                throw Error.invalidOperation(String.format(Sys.Res.scriptDependencyNotFound, scriptName, dependency));
            }
        }
    }
}
Type._registerNamespace("Sys");
Sys.__upperCaseTypes = {};
Sys.__rootNamespaces = [Sys];
Sys.__registeredTypes = {};
Sys._isInstanceOfType = function Sys$_isInstanceOfType(type, instance) {
    if (typeof(instance) === "undefined" || instance === null) return false;
    if (instance instanceof type) return true;
    var instanceType = Object.getType(instance);
    return !!(instanceType === type) ||
           (instanceType.inheritsFrom && instanceType.inheritsFrom(type)) ||
           (instanceType.implementsInterface && instanceType.implementsInterface(type));
}
Sys._getBaseMethod = function Sys$_getBaseMethod(type, instance, name) {
    if (!Sys._isInstanceOfType(type, instance)) throw Error.argumentType('instance', Object.getType(instance), type);
    var baseType = type.getBaseType();
    if (baseType) {
        var baseMethod = baseType.prototype[name];
        return (baseMethod instanceof Function) ? baseMethod : null;
    }
    return null;
}
Sys._isDomElement = function Sys$_isDomElement(obj) {
    var val = false;
    if (typeof (obj.nodeType) !== 'number') {
        var doc = obj.ownerDocument || obj.document || obj;
        if (doc != obj) {
            var w = doc.defaultView || doc.parentWindow;
            val = (w != obj);
        }
        else {
            val = (typeof (doc.body) === 'undefined');
        }
    }
    return !val;
}
 
Array.__typeName = 'Array';
Array.__class = true;
Array.add = Array.enqueue = function Array$enqueue(array, item) {
    /// <summary locid="M:J#Array.enqueue" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    array[array.length] = item;
}
Array.addRange = function Array$addRange(array, items) {
    /// <summary locid="M:J#Array.addRange" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="items" type="Array" elementMayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "items", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    array.push.apply(array, items);
}
Array.clear = function Array$clear(array) {
    /// <summary locid="M:J#Array.clear" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    array.length = 0;
}
Array.clone = function Array$clone(array) {
    /// <summary locid="M:J#Array.clone" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <returns type="Array" elementMayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    if (array.length === 1) {
        return [array[0]];
    }
    else {
        return Array.apply(null, array);
    }
}
Array.contains = function Array$contains(array, item) {
    /// <summary locid="M:J#Array.contains" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    return (Sys._indexOf(array, item) >= 0);
}
Array.dequeue = function Array$dequeue(array) {
    /// <summary locid="M:J#Array.dequeue" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <returns mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    return array.shift();
}
Array.forEach = function Array$forEach(array, method, instance) {
    /// <summary locid="M:J#Array.forEach" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="method" type="Function"></param>
    /// <param name="instance" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "method", type: Function},
        {name: "instance", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    for (var i = 0, l = array.length; i < l; i++) {
        var elt = array[i];
        if (typeof(elt) !== 'undefined') method.call(instance, elt, i, array);
    }
}
Array.indexOf = function Array$indexOf(array, item, start) {
    /// <summary locid="M:J#Array.indexOf" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" optional="true" mayBeNull="true"></param>
    /// <param name="start" optional="true" mayBeNull="true"></param>
    /// <returns type="Number"></returns>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true, optional: true},
        {name: "start", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    return Sys._indexOf(array, item, start);
}
Array.insert = function Array$insert(array, index, item) {
    /// <summary locid="M:J#Array.insert" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="index" mayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "index", mayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    array.splice(index, 0, item);
}
Array.parse = function Array$parse(value) {
    /// <summary locid="M:J#Array.parse" />
    /// <param name="value" type="String" mayBeNull="true"></param>
    /// <returns type="Array" elementMayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String, mayBeNull: true}
    ]);
    if (e) throw e;
    if (!value) return [];
    var v = eval(value);
    if (!Array.isInstanceOfType(v)) throw Error.argument('value', Sys.Res.arrayParseBadFormat);
    return v;
}
Array.remove = function Array$remove(array, item) {
    /// <summary locid="M:J#Array.remove" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    var index = Sys._indexOf(array, item);
    if (index >= 0) {
        array.splice(index, 1);
    }
    return (index >= 0);
}
Array.removeAt = function Array$removeAt(array, index) {
    /// <summary locid="M:J#Array.removeAt" />
    /// <param name="array" type="Array" elementMayBeNull="true"></param>
    /// <param name="index" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "array", type: Array, elementMayBeNull: true},
        {name: "index", mayBeNull: true}
    ]);
    if (e) throw e;
    array.splice(index, 1);
}
Sys._indexOf = function Sys$_indexOf(array, item, start) {
    if (typeof(item) === "undefined") return -1;
    var length = array.length;
    if (length !== 0) {
        start = start - 0;
        if (isNaN(start)) {
            start = 0;
        }
        else {
            if (isFinite(start)) {
                start = start - (start % 1);
            }
            if (start < 0) {
                start = Math.max(0, length + start);
            }
        }
        for (var i = start; i < length; i++) {
            if ((typeof(array[i]) !== "undefined") && (array[i] === item)) {
                return i;
            }
        }
    }
    return -1;
}
Type._registerScript._scripts = {
	"MicrosoftAjaxCore.js": true,
	"MicrosoftAjaxGlobalization.js": true,
	"MicrosoftAjaxSerialization.js": true,
	"MicrosoftAjaxComponentModel.js": true,
	"MicrosoftAjaxHistory.js": true,
	"MicrosoftAjaxNetwork.js" : true,
	"MicrosoftAjaxWebServices.js": true };
 
Sys.IDisposable = function Sys$IDisposable() {
    throw Error.notImplemented();
}
    function Sys$IDisposable$dispose() {
        throw Error.notImplemented();
    }
Sys.IDisposable.prototype = {
    dispose: Sys$IDisposable$dispose
}
Sys.IDisposable.registerInterface('Sys.IDisposable');
 
Sys.StringBuilder = function Sys$StringBuilder(initialText) {
    /// <summary locid="M:J#Sys.StringBuilder.#ctor" />
    /// <param name="initialText" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "initialText", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    this._parts = (typeof(initialText) !== 'undefined' && initialText !== null && initialText !== '') ?
        [initialText.toString()] : [];
    this._value = {};
    this._len = 0;
}
    function Sys$StringBuilder$append(text) {
        /// <summary locid="M:J#Sys.StringBuilder.append" />
        /// <param name="text" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "text", mayBeNull: true}
        ]);
        if (e) throw e;
        this._parts[this._parts.length] = text;
    }
    function Sys$StringBuilder$appendLine(text) {
        /// <summary locid="M:J#Sys.StringBuilder.appendLine" />
        /// <param name="text" optional="true" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "text", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        this._parts[this._parts.length] =
            ((typeof(text) === 'undefined') || (text === null) || (text === '')) ?
            '\r\n' : text + '\r\n';
    }
    function Sys$StringBuilder$clear() {
        /// <summary locid="M:J#Sys.StringBuilder.clear" />
        if (arguments.length !== 0) throw Error.parameterCount();
        this._parts = [];
        this._value = {};
        this._len = 0;
    }
    function Sys$StringBuilder$isEmpty() {
        /// <summary locid="M:J#Sys.StringBuilder.isEmpty" />
        /// <returns type="Boolean"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._parts.length === 0) return true;
        return this.toString() === '';
    }
    function Sys$StringBuilder$toString(separator) {
        /// <summary locid="M:J#Sys.StringBuilder.toString" />
        /// <param name="separator" type="String" optional="true" mayBeNull="true"></param>
        /// <returns type="String"></returns>
        var e = Function._validateParams(arguments, [
            {name: "separator", type: String, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        separator = separator || '';
        var parts = this._parts;
        if (this._len !== parts.length) {
            this._value = {};
            this._len = parts.length;
        }
        var val = this._value;
        if (typeof(val[separator]) === 'undefined') {
            if (separator !== '') {
                for (var i = 0; i < parts.length;) {
                    if ((typeof(parts[i]) === 'undefined') || (parts[i] === '') || (parts[i] === null)) {
                        parts.splice(i, 1);
                    }
                    else {
                        i++;
                    }
                }
            }
            val[separator] = this._parts.join(separator);
        }
        return val[separator];
    }
Sys.StringBuilder.prototype = {
    append: Sys$StringBuilder$append,
    appendLine: Sys$StringBuilder$appendLine,
    clear: Sys$StringBuilder$clear,
    isEmpty: Sys$StringBuilder$isEmpty,
    toString: Sys$StringBuilder$toString
}
Sys.StringBuilder.registerClass('Sys.StringBuilder');
 
Sys.Browser = {};
Sys.Browser.InternetExplorer = {};
Sys.Browser.Firefox = {};
Sys.Browser.Safari = {};
Sys.Browser.Opera = {};
Sys.Browser.agent = null;
Sys.Browser.hasDebuggerStatement = false;
Sys.Browser.name = navigator.appName;
Sys.Browser.version = parseFloat(navigator.appVersion);
Sys.Browser.documentMode = 0;
if (navigator.userAgent.indexOf(' MSIE ') > -1) {
    Sys.Browser.agent = Sys.Browser.InternetExplorer;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/MSIE (\d+\.\d+)/)[1]);
    if (Sys.Browser.version >= 8) {
        if (document.documentMode >= 7) {
            Sys.Browser.documentMode = document.documentMode;    
        }
    }
    Sys.Browser.hasDebuggerStatement = true;
}
else if (navigator.userAgent.indexOf(' Firefox/') > -1) {
    Sys.Browser.agent = Sys.Browser.Firefox;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/ Firefox\/(\d+\.\d+)/)[1]);
    Sys.Browser.name = 'Firefox';
    Sys.Browser.hasDebuggerStatement = true;
}
else if (navigator.userAgent.indexOf(' AppleWebKit/') > -1) {
    Sys.Browser.agent = Sys.Browser.Safari;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/ AppleWebKit\/(\d+(\.\d+)?)/)[1]);
    Sys.Browser.name = 'Safari';
}
else if (navigator.userAgent.indexOf('Opera/') > -1) {
    Sys.Browser.agent = Sys.Browser.Opera;
}
 
Sys.EventArgs = function Sys$EventArgs() {
    /// <summary locid="M:J#Sys.EventArgs.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
}
Sys.EventArgs.registerClass('Sys.EventArgs');
Sys.EventArgs.Empty = new Sys.EventArgs();
 
Sys.CancelEventArgs = function Sys$CancelEventArgs() {
    /// <summary locid="M:J#Sys.CancelEventArgs.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    Sys.CancelEventArgs.initializeBase(this);
    this._cancel = false;
}
    function Sys$CancelEventArgs$get_cancel() {
        /// <value type="Boolean" locid="P:J#Sys.CancelEventArgs.cancel"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._cancel;
    }
    function Sys$CancelEventArgs$set_cancel(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._cancel = value;
    }
Sys.CancelEventArgs.prototype = {
    get_cancel: Sys$CancelEventArgs$get_cancel,
    set_cancel: Sys$CancelEventArgs$set_cancel
}
Sys.CancelEventArgs.registerClass('Sys.CancelEventArgs', Sys.EventArgs);
Type.registerNamespace('Sys.UI');
 
Sys._Debug = function Sys$_Debug() {
    /// <summary locid="M:J#Sys.Debug.#ctor" />
    /// <field name="isDebug" type="Boolean" locid="F:J#Sys.Debug.isDebug"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
}
    function Sys$_Debug$_appendConsole(text) {
        if ((typeof(Debug) !== 'undefined') && Debug.writeln) {
            Debug.writeln(text);
        }
        if (window.console && window.console.log) {
            window.console.log(text);
        }
        if (window.opera) {
            window.opera.postError(text);
        }
        if (window.debugService) {
            window.debugService.trace(text);
        }
    }
    function Sys$_Debug$_appendTrace(text) {
        var traceElement = document.getElementById('TraceConsole');
        if (traceElement && (traceElement.tagName.toUpperCase() === 'TEXTAREA')) {
            traceElement.value += text + '\n';
        }
    }
    function Sys$_Debug$assert(condition, message, displayCaller) {
        /// <summary locid="M:J#Sys.Debug.assert" />
        /// <param name="condition" type="Boolean"></param>
        /// <param name="message" type="String" optional="true" mayBeNull="true"></param>
        /// <param name="displayCaller" type="Boolean" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "condition", type: Boolean},
            {name: "message", type: String, mayBeNull: true, optional: true},
            {name: "displayCaller", type: Boolean, optional: true}
        ]);
        if (e) throw e;
        if (!condition) {
            message = (displayCaller && this.assert.caller) ?
                String.format(Sys.Res.assertFailedCaller, message, this.assert.caller) :
                String.format(Sys.Res.assertFailed, message);
            if (confirm(String.format(Sys.Res.breakIntoDebugger, message))) {
                this.fail(message);
            }
        }
    }
    function Sys$_Debug$clearTrace() {
        /// <summary locid="M:J#Sys.Debug.clearTrace" />
        if (arguments.length !== 0) throw Error.parameterCount();
        var traceElement = document.getElementById('TraceConsole');
        if (traceElement && (traceElement.tagName.toUpperCase() === 'TEXTAREA')) {
            traceElement.value = '';
        }
    }
    function Sys$_Debug$fail(message) {
        /// <summary locid="M:J#Sys.Debug.fail" />
        /// <param name="message" type="String" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "message", type: String, mayBeNull: true}
        ]);
        if (e) throw e;
        this._appendConsole(message);
        if (Sys.Browser.hasDebuggerStatement) {
            eval('debugger');
        }
    }
    function Sys$_Debug$trace(text) {
        /// <summary locid="M:J#Sys.Debug.trace" />
        /// <param name="text"></param>
        var e = Function._validateParams(arguments, [
            {name: "text"}
        ]);
        if (e) throw e;
        this._appendConsole(text);
        this._appendTrace(text);
    }
    function Sys$_Debug$traceDump(object, name) {
        /// <summary locid="M:J#Sys.Debug.traceDump" />
        /// <param name="object" mayBeNull="true"></param>
        /// <param name="name" type="String" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "object", mayBeNull: true},
            {name: "name", type: String, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        var text = this._traceDump(object, name, true);
    }
    function Sys$_Debug$_traceDump(object, name, recursive, indentationPadding, loopArray) {
        name = name? name : 'traceDump';
        indentationPadding = indentationPadding? indentationPadding : '';
        if (object === null) {
            this.trace(indentationPadding + name + ': null');
            return;
        }
        switch(typeof(object)) {
            case 'undefined':
                this.trace(indentationPadding + name + ': Undefined');
                break;
            case 'number': case 'string': case 'boolean':
                this.trace(indentationPadding + name + ': ' + object);
                break;
            default:
                if (Date.isInstanceOfType(object) || RegExp.isInstanceOfType(object)) {
                    this.trace(indentationPadding + name + ': ' + object.toString());
                    break;
                }
                if (!loopArray) {
                    loopArray = [];
                }
                else if (Array.contains(loopArray, object)) {
                    this.trace(indentationPadding + name + ': ...');
                    return;
                }
                Array.add(loopArray, object);
                if ((object == window) || (object === document) ||
                    (window.HTMLElement && (object instanceof HTMLElement)) ||
                    (typeof(object.nodeName) === 'string')) {
                    var tag = object.tagName? object.tagName : 'DomElement';
                    if (object.id) {
                        tag += ' - ' + object.id;
                    }
                    this.trace(indentationPadding + name + ' {' +  tag + '}');
                }
                else {
                    var typeName = Object.getTypeName(object);
                    this.trace(indentationPadding + name + (typeof(typeName) === 'string' ? ' {' + typeName + '}' : ''));
                    if ((indentationPadding === '') || recursive) {
                        indentationPadding += "    ";
                        var i, length, properties, p, v;
                        if (Array.isInstanceOfType(object)) {
                            length = object.length;
                            for (i = 0; i < length; i++) {
                                this._traceDump(object[i], '[' + i + ']', recursive, indentationPadding, loopArray);
                            }
                        }
                        else {
                            for (p in object) {
                                v = object[p];
                                if (!Function.isInstanceOfType(v)) {
                                    this._traceDump(v, p, recursive, indentationPadding, loopArray);
                                }
                            }
                        }
                    }
                }
                Array.remove(loopArray, object);
        }
    }
Sys._Debug.prototype = {
    _appendConsole: Sys$_Debug$_appendConsole,
    _appendTrace: Sys$_Debug$_appendTrace,
    assert: Sys$_Debug$assert,
    clearTrace: Sys$_Debug$clearTrace,
    fail: Sys$_Debug$fail,
    trace: Sys$_Debug$trace,
    traceDump: Sys$_Debug$traceDump,
    _traceDump: Sys$_Debug$_traceDump
}
Sys._Debug.registerClass('Sys._Debug');
Sys.Debug = new Sys._Debug();
    Sys.Debug.isDebug = true;
 
function Sys$Enum$parse(value, ignoreCase) {
    /// <summary locid="M:J#Sys.Enum.parse" />
    /// <param name="value" type="String"></param>
    /// <param name="ignoreCase" type="Boolean" optional="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String},
        {name: "ignoreCase", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    var values, parsed, val;
    if (ignoreCase) {
        values = this.__lowerCaseValues;
        if (!values) {
            this.__lowerCaseValues = values = {};
            var prototype = this.prototype;
            for (var name in prototype) {
                values[name.toLowerCase()] = prototype[name];
            }
        }
    }
    else {
        values = this.prototype;
    }
    if (!this.__flags) {
        val = (ignoreCase ? value.toLowerCase() : value);
        parsed = values[val.trim()];
        if (typeof(parsed) !== 'number') throw Error.argument('value', String.format(Sys.Res.enumInvalidValue, value, this.__typeName));
        return parsed;
    }
    else {
        var parts = (ignoreCase ? value.toLowerCase() : value).split(',');
        var v = 0;
        for (var i = parts.length - 1; i >= 0; i--) {
            var part = parts[i].trim();
            parsed = values[part];
            if (typeof(parsed) !== 'number') throw Error.argument('value', String.format(Sys.Res.enumInvalidValue, value.split(',')[i].trim(), this.__typeName));
            v |= parsed;
        }
        return v;
    }
}
function Sys$Enum$toString(value) {
    /// <summary locid="M:J#Sys.Enum.toString" />
    /// <param name="value" optional="true" mayBeNull="true"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    if ((typeof(value) === 'undefined') || (value === null)) return this.__string;
    if ((typeof(value) != 'number') || ((value % 1) !== 0)) throw Error.argumentType('value', Object.getType(value), this);
    var values = this.prototype;
    var i;
    if (!this.__flags || (value === 0)) {
        for (i in values) {
            if (values[i] === value) {
                return i;
            }
        }
    }
    else {
        var sorted = this.__sortedValues;
        if (!sorted) {
            sorted = [];
            for (i in values) {
                sorted[sorted.length] = {key: i, value: values[i]};
            }
            sorted.sort(function(a, b) {
                return a.value - b.value;
            });
            this.__sortedValues = sorted;
        }
        var parts = [];
        var v = value;
        for (i = sorted.length - 1; i >= 0; i--) {
            var kvp = sorted[i];
            var vali = kvp.value;
            if (vali === 0) continue;
            if ((vali & value) === vali) {
                parts[parts.length] = kvp.key;
                v -= vali;
                if (v === 0) break;
            }
        }
        if (parts.length && v === 0) return parts.reverse().join(', ');
    }
    throw Error.argumentOutOfRange('value', value, String.format(Sys.Res.enumInvalidValue, value, this.__typeName));
}
Type.prototype.registerEnum = function Type$registerEnum(name, flags) {
    /// <summary locid="M:J#Sys.UI.LineType.#ctor" />
    /// <param name="name" type="String"></param>
    /// <param name="flags" type="Boolean" optional="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "name", type: String},
        {name: "flags", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    if (!Type.__fullyQualifiedIdentifierRegExp.test(name)) throw Error.argument('name', Sys.Res.notATypeName);
    var parsedName;
    try {
        parsedName = eval(name);
    }
    catch(e) {
        throw Error.argument('name', Sys.Res.argumentTypeName);
    }
    if (parsedName !== this) throw Error.argument('name', Sys.Res.badTypeName);
    if (Sys.__registeredTypes[name]) throw Error.invalidOperation(String.format(Sys.Res.typeRegisteredTwice, name));
    for (var j in this.prototype) {
        var val = this.prototype[j];
        if (!Type.__identifierRegExp.test(j)) throw Error.invalidOperation(String.format(Sys.Res.enumInvalidValueName, j));
        if (typeof(val) !== 'number' || (val % 1) !== 0) throw Error.invalidOperation(Sys.Res.enumValueNotInteger);
        if (typeof(this[j]) !== 'undefined') throw Error.invalidOperation(String.format(Sys.Res.enumReservedName, j));
    }
    Sys.__upperCaseTypes[name.toUpperCase()] = this;
    for (var i in this.prototype) {
        this[i] = this.prototype[i];
    }
    this.__typeName = name;
    this.parse = Sys$Enum$parse;
    this.__string = this.toString();
    this.toString = Sys$Enum$toString;
    this.__flags = flags;
    this.__enum = true;
    Sys.__registeredTypes[name] = true;
}
Type.isEnum = function Type$isEnum(type) {
    /// <summary locid="M:J#Type.isEnum" />
    /// <param name="type" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", mayBeNull: true}
    ]);
    if (e) throw e;
    if ((typeof(type) === 'undefined') || (type === null)) return false;
    return !!type.__enum;
}
Type.isFlags = function Type$isFlags(type) {
    /// <summary locid="M:J#Type.isFlags" />
    /// <param name="type" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", mayBeNull: true}
    ]);
    if (e) throw e;
    if ((typeof(type) === 'undefined') || (type === null)) return false;
    return !!type.__flags;
}
Sys.CollectionChange = function Sys$CollectionChange(action, newItems, newStartingIndex, oldItems, oldStartingIndex) {
    /// <summary locid="M:J#Sys.CollectionChange.#ctor" />
    /// <param name="action" type="Sys.NotifyCollectionChangedAction"></param>
    /// <param name="newItems" optional="true" mayBeNull="true"></param>
    /// <param name="newStartingIndex" type="Number" integer="true" optional="true" mayBeNull="true"></param>
    /// <param name="oldItems" optional="true" mayBeNull="true"></param>
    /// <param name="oldStartingIndex" type="Number" integer="true" optional="true" mayBeNull="true"></param>
    /// <field name="action" type="Sys.NotifyCollectionChangedAction" locid="F:J#Sys.CollectionChange.action"></field>
    /// <field name="newItems" type="Array" mayBeNull="true" elementMayBeNull="true" locid="F:J#Sys.CollectionChange.newItems"></field>
    /// <field name="newStartingIndex" type="Number" integer="true" locid="F:J#Sys.CollectionChange.newStartingIndex"></field>
    /// <field name="oldItems" type="Array" mayBeNull="true" elementMayBeNull="true" locid="F:J#Sys.CollectionChange.oldItems"></field>
    /// <field name="oldStartingIndex" type="Number" integer="true" locid="F:J#Sys.CollectionChange.oldStartingIndex"></field>
    var e = Function._validateParams(arguments, [
        {name: "action", type: Sys.NotifyCollectionChangedAction},
        {name: "newItems", mayBeNull: true, optional: true},
        {name: "newStartingIndex", type: Number, mayBeNull: true, integer: true, optional: true},
        {name: "oldItems", mayBeNull: true, optional: true},
        {name: "oldStartingIndex", type: Number, mayBeNull: true, integer: true, optional: true}
    ]);
    if (e) throw e;
    this.action = action;
    if (newItems) {
        if (!(newItems instanceof Array)) {
            newItems = [newItems];
        }
    }
    this.newItems = newItems || null;
    if (typeof newStartingIndex !== "number") {
        newStartingIndex = -1;
    }
    this.newStartingIndex = newStartingIndex;
    if (oldItems) {
        if (!(oldItems instanceof Array)) {
            oldItems = [oldItems];
        }
    }
    this.oldItems = oldItems || null;
    if (typeof oldStartingIndex !== "number") {
        oldStartingIndex = -1;
    }
    this.oldStartingIndex = oldStartingIndex;
}
Sys.CollectionChange.registerClass("Sys.CollectionChange");
Sys.NotifyCollectionChangedAction = function Sys$NotifyCollectionChangedAction() {
    /// <summary locid="M:J#Sys.NotifyCollectionChangedAction.#ctor" />
    /// <field name="add" type="Number" integer="true" static="true" locid="F:J#Sys.NotifyCollectionChangedAction.add"></field>
    /// <field name="remove" type="Number" integer="true" static="true" locid="F:J#Sys.NotifyCollectionChangedAction.remove"></field>
    /// <field name="reset" type="Number" integer="true" static="true" locid="F:J#Sys.NotifyCollectionChangedAction.reset"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
Sys.NotifyCollectionChangedAction.prototype = {
    add: 0,
    remove: 1,
    reset: 2
}
Sys.NotifyCollectionChangedAction.registerEnum('Sys.NotifyCollectionChangedAction');
Sys.NotifyCollectionChangedEventArgs = function Sys$NotifyCollectionChangedEventArgs(changes) {
    /// <summary locid="M:J#Sys.NotifyCollectionChangedEventArgs.#ctor" />
    /// <param name="changes" type="Array" elementType="Sys.CollectionChange"></param>
    var e = Function._validateParams(arguments, [
        {name: "changes", type: Array, elementType: Sys.CollectionChange}
    ]);
    if (e) throw e;
    this._changes = changes;
    Sys.NotifyCollectionChangedEventArgs.initializeBase(this);
}
    function Sys$NotifyCollectionChangedEventArgs$get_changes() {
        /// <value type="Array" elementType="Sys.CollectionChange" locid="P:J#Sys.NotifyCollectionChangedEventArgs.changes"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._changes || [];
    }
Sys.NotifyCollectionChangedEventArgs.prototype = {
    get_changes: Sys$NotifyCollectionChangedEventArgs$get_changes
}
Sys.NotifyCollectionChangedEventArgs.registerClass("Sys.NotifyCollectionChangedEventArgs", Sys.EventArgs);
Sys.Observer = function Sys$Observer() {
    throw Error.invalidOperation();
}
Sys.Observer.registerClass("Sys.Observer");
Sys.Observer.makeObservable = function Sys$Observer$makeObservable(target) {
    /// <summary locid="M:J#Sys.Observer.makeObservable" />
    /// <param name="target" mayBeNull="false"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "target"}
    ]);
    if (e) throw e;
    var isArray = target instanceof Array,
        o = Sys.Observer;
    Sys.Observer._ensureObservable(target);
    if (target.setValue === o._observeMethods.setValue) return target;
    o._addMethods(target, o._observeMethods);
    if (isArray) {
        o._addMethods(target, o._arrayMethods);
    }
    return target;
}
Sys.Observer._ensureObservable = function Sys$Observer$_ensureObservable(target) {
    var type = typeof target;
    if ((type === "string") || (type === "number") || (type === "boolean") || (type === "date")) {
        throw Error.invalidOperation(String.format(Sys.Res.notObservable, type));
    }
}
Sys.Observer._addMethods = function Sys$Observer$_addMethods(target, methods) {
    for (var m in methods) {
        if (target[m] && (target[m] !== methods[m])) {
            throw Error.invalidOperation(String.format(Sys.Res.observableConflict, m));
        }
        target[m] = methods[m];
    }
}
Sys.Observer._addEventHandler = function Sys$Observer$_addEventHandler(target, eventName, handler) {
    Sys.Observer._getContext(target, true).events._addHandler(eventName, handler);
}
Sys.Observer.addEventHandler = function Sys$Observer$addEventHandler(target, eventName, handler) {
    /// <summary locid="M:J#Sys.Observer.addEventHandler" />
    /// <param name="target"></param>
    /// <param name="eventName" type="String"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "eventName", type: String},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._addEventHandler(target, eventName, handler);
}
Sys.Observer._removeEventHandler = function Sys$Observer$_removeEventHandler(target, eventName, handler) {
    Sys.Observer._getContext(target, true).events._removeHandler(eventName, handler);
}
Sys.Observer.removeEventHandler = function Sys$Observer$removeEventHandler(target, eventName, handler) {
    /// <summary locid="M:J#Sys.Observer.removeEventHandler" />
    /// <param name="target"></param>
    /// <param name="eventName" type="String"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "eventName", type: String},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._removeEventHandler(target, eventName, handler);
}
Sys.Observer.raiseEvent = function Sys$Observer$raiseEvent(target, eventName, eventArgs) {
    /// <summary locid="M:J#Sys.Observer.raiseEvent" />
    /// <param name="target"></param>
    /// <param name="eventName" type="String"></param>
    /// <param name="eventArgs" type="Sys.EventArgs"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "eventName", type: String},
        {name: "eventArgs", type: Sys.EventArgs}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    var ctx = Sys.Observer._getContext(target);
    if (!ctx) return;
    var handler = ctx.events.getHandler(eventName);
    if (handler) {
        handler(target, eventArgs);
    }
}
Sys.Observer.addPropertyChanged = function Sys$Observer$addPropertyChanged(target, handler) {
    /// <summary locid="M:J#Sys.Observer.addPropertyChanged" />
    /// <param name="target" mayBeNull="false"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._addEventHandler(target, "propertyChanged", handler);
}
Sys.Observer.removePropertyChanged = function Sys$Observer$removePropertyChanged(target, handler) {
    /// <summary locid="M:J#Sys.Observer.removePropertyChanged" />
    /// <param name="target" mayBeNull="false"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._removeEventHandler(target, "propertyChanged", handler);
}
Sys.Observer.beginUpdate = function Sys$Observer$beginUpdate(target) {
    /// <summary locid="M:J#Sys.Observer.beginUpdate" />
    /// <param name="target" mayBeNull="false"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._getContext(target, true).updating = true;
}
Sys.Observer.endUpdate = function Sys$Observer$endUpdate(target) {
    /// <summary locid="M:J#Sys.Observer.endUpdate" />
    /// <param name="target" mayBeNull="false"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    var ctx = Sys.Observer._getContext(target);
    if (!ctx || !ctx.updating) return;
    ctx.updating = false;
    var dirty = ctx.dirty;
    ctx.dirty = false;
    if (dirty) {
        if (target instanceof Array) {
            var changes = ctx.changes;
            ctx.changes = null;
            Sys.Observer.raiseCollectionChanged(target, changes);
        }
        Sys.Observer.raisePropertyChanged(target, "");
    }
}
Sys.Observer.isUpdating = function Sys$Observer$isUpdating(target) {
    /// <summary locid="M:J#Sys.Observer.isUpdating" />
    /// <param name="target" mayBeNull="false"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "target"}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    var ctx = Sys.Observer._getContext(target);
    return ctx ? ctx.updating : false;
}
Sys.Observer._setValue = function Sys$Observer$_setValue(target, propertyName, value) {
    var getter, setter, mainTarget = target, path = propertyName.split('.');
    for (var i = 0, l = (path.length - 1); i < l ; i++) {
        var name = path[i];
        getter = target["get_" + name]; 
        if (typeof (getter) === "function") {
            target = getter.call(target);
        }
        else {
            target = target[name];
        }
        var type = typeof (target);
        if ((target === null) || (type === "undefined")) {
            throw Error.invalidOperation(String.format(Sys.Res.nullReferenceInPath, propertyName));
        }
    }    
    var currentValue, lastPath = path[l];
    getter = target["get_" + lastPath];
    setter = target["set_" + lastPath];
    if (typeof(getter) === 'function') {
        currentValue = getter.call(target);
    }
    else {
        currentValue = target[lastPath];
    }
    if (typeof(setter) === 'function') {
        setter.call(target, value);
    }
    else {
        target[lastPath] = value;
    }
    if (currentValue !== value) {
        var ctx = Sys.Observer._getContext(mainTarget);
        if (ctx && ctx.updating) {
            ctx.dirty = true;
            return;
        };
        Sys.Observer.raisePropertyChanged(mainTarget, path[0]);
    }
}
Sys.Observer.setValue = function Sys$Observer$setValue(target, propertyName, value) {
    /// <summary locid="M:J#Sys.Observer.setValue" />
    /// <param name="target" mayBeNull="false"></param>
    /// <param name="propertyName" type="String"></param>
    /// <param name="value" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "propertyName", type: String},
        {name: "value", mayBeNull: true}
    ]);
    if (e) throw e;
    Sys.Observer._ensureObservable(target);
    Sys.Observer._setValue(target, propertyName, value);
}
Sys.Observer.raisePropertyChanged = function Sys$Observer$raisePropertyChanged(target, propertyName) {
    /// <summary locid="M:J#Sys.Observer.raisePropertyChanged" />
    /// <param name="target" mayBeNull="false"></param>
    /// <param name="propertyName" type="String"></param>
    Sys.Observer.raiseEvent(target, "propertyChanged", new Sys.PropertyChangedEventArgs(propertyName));
}
Sys.Observer.addCollectionChanged = function Sys$Observer$addCollectionChanged(target, handler) {
    /// <summary locid="M:J#Sys.Observer.addCollectionChanged" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._addEventHandler(target, "collectionChanged", handler);
}
Sys.Observer.removeCollectionChanged = function Sys$Observer$removeCollectionChanged(target, handler) {
    /// <summary locid="M:J#Sys.Observer.removeCollectionChanged" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.Observer._removeEventHandler(target, "collectionChanged", handler);
}
Sys.Observer._collectionChange = function Sys$Observer$_collectionChange(target, change) {
    var ctx = Sys.Observer._getContext(target);
    if (ctx && ctx.updating) {
        ctx.dirty = true;
        var changes = ctx.changes;
        if (!changes) {
            ctx.changes = changes = [change];
        }
        else {
            changes.push(change);
        }
    }
    else {
        Sys.Observer.raiseCollectionChanged(target, [change]);
        Sys.Observer.raisePropertyChanged(target, 'length');
    }
}
Sys.Observer.add = function Sys$Observer$add(target, item) {
    /// <summary locid="M:J#Sys.Observer.add" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    var change = new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.add, [item], target.length);
    Array.add(target, item);
    Sys.Observer._collectionChange(target, change);
}
Sys.Observer.addRange = function Sys$Observer$addRange(target, items) {
    /// <summary locid="M:J#Sys.Observer.addRange" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="items" type="Array" elementMayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "items", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    var change = new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.add, items, target.length);
    Array.addRange(target, items);
    Sys.Observer._collectionChange(target, change);
}
Sys.Observer.clear = function Sys$Observer$clear(target) {
    /// <summary locid="M:J#Sys.Observer.clear" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true}
    ]);
    if (e) throw e;
    var oldItems = Array.clone(target);
    Array.clear(target);
    Sys.Observer._collectionChange(target, new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.reset, null, -1, oldItems, 0));
}
Sys.Observer.insert = function Sys$Observer$insert(target, index, item) {
    /// <summary locid="M:J#Sys.Observer.insert" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="index" type="Number" integer="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "index", type: Number, integer: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    Array.insert(target, index, item);
    Sys.Observer._collectionChange(target, new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.add, [item], index));
}
Sys.Observer.remove = function Sys$Observer$remove(target, item) {
    /// <summary locid="M:J#Sys.Observer.remove" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="item" mayBeNull="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "item", mayBeNull: true}
    ]);
    if (e) throw e;
    var index = Array.indexOf(target, item);
    if (index !== -1) {
        Array.remove(target, item);
        Sys.Observer._collectionChange(target, new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.remove, null, -1, [item], index));
        return true;
    }
    return false;
}
Sys.Observer.removeAt = function Sys$Observer$removeAt(target, index) {
    /// <summary locid="M:J#Sys.Observer.removeAt" />
    /// <param name="target" type="Array" elementMayBeNull="true"></param>
    /// <param name="index" type="Number" integer="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", type: Array, elementMayBeNull: true},
        {name: "index", type: Number, integer: true}
    ]);
    if (e) throw e;
    if ((index > -1) && (index < target.length)) {
        var item = target[index];
        Array.removeAt(target, index);
        Sys.Observer._collectionChange(target, new Sys.CollectionChange(Sys.NotifyCollectionChangedAction.remove, null, -1, [item], index));
    }
}
Sys.Observer.raiseCollectionChanged = function Sys$Observer$raiseCollectionChanged(target, changes) {
    /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
    /// <param name="target"></param>
    /// <param name="changes" type="Array" elementType="Sys.CollectionChange"></param>
    Sys.Observer.raiseEvent(target, "collectionChanged", new Sys.NotifyCollectionChangedEventArgs(changes));
}
Sys.Observer._observeMethods = {
    add_propertyChanged: function(handler) {
        Sys.Observer._addEventHandler(this, "propertyChanged", handler);
    },
    remove_propertyChanged: function(handler) {
        Sys.Observer._removeEventHandler(this, "propertyChanged", handler);
    },
    addEventHandler: function(eventName, handler) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="eventName" type="String"></param>
        /// <param name="handler" type="Function"></param>
        var e = Function._validateParams(arguments, [
            {name: "eventName", type: String},
            {name: "handler", type: Function}
        ]);
        if (e) throw e;
        Sys.Observer._addEventHandler(this, eventName, handler);
    },
    removeEventHandler: function(eventName, handler) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="eventName" type="String"></param>
        /// <param name="handler" type="Function"></param>
        var e = Function._validateParams(arguments, [
            {name: "eventName", type: String},
            {name: "handler", type: Function}
        ]);
        if (e) throw e;
        Sys.Observer._removeEventHandler(this, eventName, handler);
    },
    get_isUpdating: function() {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <returns type="Boolean"></returns>
        return Sys.Observer.isUpdating(this);
    },
    beginUpdate: function() {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        Sys.Observer.beginUpdate(this);
    },
    endUpdate: function() {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        Sys.Observer.endUpdate(this);
    },
    setValue: function(name, value) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="name" type="String"></param>
        /// <param name="value" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "name", type: String},
            {name: "value", mayBeNull: true}
        ]);
        if (e) throw e;
        Sys.Observer._setValue(this, name, value);
    },
    raiseEvent: function(eventName, eventArgs) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="eventName" type="String"></param>
        /// <param name="eventArgs" type="Sys.EventArgs"></param>
        Sys.Observer.raiseEvent(this, eventName, eventArgs);
    },
    raisePropertyChanged: function(name) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="name" type="String"></param>
        Sys.Observer.raiseEvent(this, "propertyChanged", new Sys.PropertyChangedEventArgs(name));
    }
}
Sys.Observer._arrayMethods = {
    add_collectionChanged: function(handler) {
        Sys.Observer._addEventHandler(this, "collectionChanged", handler);
    },
    remove_collectionChanged: function(handler) {
        Sys.Observer._removeEventHandler(this, "collectionChanged", handler);
    },
    add: function(item) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="item" mayBeNull="true"></param>
        Sys.Observer.add(this, item);
    },
    addRange: function(items) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="items" type="Array" elementMayBeNull="true"></param>
        Sys.Observer.addRange(this, items);
    },
    clear: function() {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        Sys.Observer.clear(this);
    },
    insert: function(index, item) { 
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="index" type="Number" integer="true"></param>
        /// <param name="item" mayBeNull="true"></param>
        Sys.Observer.insert(this, index, item);
    },
    remove: function(item) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="item" mayBeNull="true"></param>
        /// <returns type="Boolean"></returns>
        return Sys.Observer.remove(this, item);
    },
    removeAt: function(index) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="index" type="Number" integer="true"></param>
        Sys.Observer.removeAt(this, index);
    },
    raiseCollectionChanged: function(changes) {
        /// <summary locid="M:J#Sys.Observer.raiseCollectionChanged" />
        /// <param name="changes" type="Array" elementType="Sys.CollectionChange"></param>
        Sys.Observer.raiseEvent(this, "collectionChanged", new Sys.NotifyCollectionChangedEventArgs(changes));
    }
}
Sys.Observer._getContext = function Sys$Observer$_getContext(obj, create) {
    var ctx = obj._observerContext;
    if (ctx) return ctx();
    if (create) {
        return (obj._observerContext = Sys.Observer._createContext())();
    }
    return null;
}
Sys.Observer._createContext = function Sys$Observer$_createContext() {
    var ctx = {
        events: new Sys.EventHandlerList()
    };
    return function() {
        return ctx;
    }
}
Date._appendPreOrPostMatch = function Date$_appendPreOrPostMatch(preMatch, strBuilder) {
    var quoteCount = 0;
    var escaped = false;
    for (var i = 0, il = preMatch.length; i < il; i++) {
        var c = preMatch.charAt(i);
        switch (c) {
        case '\'':
            if (escaped) strBuilder.append("'");
            else quoteCount++;
            escaped = false;
            break;
        case '\\':
            if (escaped) strBuilder.append("\\");
            escaped = !escaped;
            break;
        default:
            strBuilder.append(c);
            escaped = false;
            break;
        }
    }
    return quoteCount;
}
Date._expandFormat = function Date$_expandFormat(dtf, format) {
    if (!format) {
        format = "F";
    }
    var len = format.length;
    if (len === 1) {
        switch (format) {
        case "d":
            return dtf.ShortDatePattern;
        case "D":
            return dtf.LongDatePattern;
        case "t":
            return dtf.ShortTimePattern;
        case "T":
            return dtf.LongTimePattern;
        case "f":
            return dtf.LongDatePattern + " " + dtf.ShortTimePattern;
        case "F":
            return dtf.FullDateTimePattern;
        case "M": case "m":
            return dtf.MonthDayPattern;
        case "s":
            return dtf.SortableDateTimePattern;
        case "Y": case "y":
            return dtf.YearMonthPattern;
        default:
            throw Error.format(Sys.Res.formatInvalidString);
        }
    }
    else if ((len === 2) && (format.charAt(0) === "%")) {
        format = format.charAt(1);
    }
    return format;
}
Date._expandYear = function Date$_expandYear(dtf, year) {
    var now = new Date(),
        era = Date._getEra(now);
    if (year < 100) {
        var curr = Date._getEraYear(now, dtf, era);
        year += curr - (curr % 100);
        if (year > dtf.Calendar.TwoDigitYearMax) {
            year -= 100;
        }
    }
    return year;
}
Date._getEra = function Date$_getEra(date, eras) {
    if (!eras) return 0;
    var start, ticks = date.getTime();
    for (var i = 0, l = eras.length; i < l; i += 4) {
        start = eras[i+2];
        if ((start === null) || (ticks >= start)) {
            return i;
        }
    }
    return 0;
}
Date._getEraYear = function Date$_getEraYear(date, dtf, era, sortable) {
    var year = date.getFullYear();
    if (!sortable && dtf.eras) {
        year -= dtf.eras[era + 3];
    }    
    return year;
}
Date._getParseRegExp = function Date$_getParseRegExp(dtf, format) {
    if (!dtf._parseRegExp) {
        dtf._parseRegExp = {};
    }
    else if (dtf._parseRegExp[format]) {
        return dtf._parseRegExp[format];
    }
    var expFormat = Date._expandFormat(dtf, format);
    expFormat = expFormat.replace(/([\^\$\.\*\+\?\|\[\]\(\)\{\}])/g, "\\\\$1");
    var regexp = new Sys.StringBuilder("^");
    var groups = [];
    var index = 0;
    var quoteCount = 0;
    var tokenRegExp = Date._getTokenRegExp();
    var match;
    while ((match = tokenRegExp.exec(expFormat)) !== null) {
        var preMatch = expFormat.slice(index, match.index);
        index = tokenRegExp.lastIndex;
        quoteCount += Date._appendPreOrPostMatch(preMatch, regexp);
        if ((quoteCount%2) === 1) {
            regexp.append(match[0]);
            continue;
        }
        switch (match[0]) {
            case 'dddd': case 'ddd':
            case 'MMMM': case 'MMM':
            case 'gg': case 'g':
                regexp.append("(\\D+)");
                break;
            case 'tt': case 't':
                regexp.append("(\\D*)");
                break;
            case 'yyyy':
                regexp.append("(\\d{4})");
                break;
            case 'fff':
                regexp.append("(\\d{3})");
                break;
            case 'ff':
                regexp.append("(\\d{2})");
                break;
            case 'f':
                regexp.append("(\\d)");
                break;
            case 'dd': case 'd':
            case 'MM': case 'M':
            case 'yy': case 'y':
            case 'HH': case 'H':
            case 'hh': case 'h':
            case 'mm': case 'm':
            case 'ss': case 's':
                regexp.append("(\\d\\d?)");
                break;
            case 'zzz':
                regexp.append("([+-]?\\d\\d?:\\d{2})");
                break;
            case 'zz': case 'z':
                regexp.append("([+-]?\\d\\d?)");
                break;
            case '/':
                regexp.append("(\\" + dtf.DateSeparator + ")");
                break;
            default:
                Sys.Debug.fail("Invalid date format pattern");
        }
        Array.add(groups, match[0]);
    }
    Date._appendPreOrPostMatch(expFormat.slice(index), regexp);
    regexp.append("$");
    var regexpStr = regexp.toString().replace(/\s+/g, "\\s+");
    var parseRegExp = {'regExp': regexpStr, 'groups': groups};
    dtf._parseRegExp[format] = parseRegExp;
    return parseRegExp;
}
Date._getTokenRegExp = function Date$_getTokenRegExp() {
    return /\/|dddd|ddd|dd|d|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f|zzz|zz|z|gg|g/g;
}
Date.parseLocale = function Date$parseLocale(value, formats) {
    /// <summary locid="M:J#Date.parseLocale" />
    /// <param name="value" type="String"></param>
    /// <param name="formats" parameterArray="true" optional="true" mayBeNull="true"></param>
    /// <returns type="Date"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String},
        {name: "formats", mayBeNull: true, optional: true, parameterArray: true}
    ]);
    if (e) throw e;
    return Date._parse(value, Sys.CultureInfo.CurrentCulture, arguments);
}
Date.parseInvariant = function Date$parseInvariant(value, formats) {
    /// <summary locid="M:J#Date.parseInvariant" />
    /// <param name="value" type="String"></param>
    /// <param name="formats" parameterArray="true" optional="true" mayBeNull="true"></param>
    /// <returns type="Date"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String},
        {name: "formats", mayBeNull: true, optional: true, parameterArray: true}
    ]);
    if (e) throw e;
    return Date._parse(value, Sys.CultureInfo.InvariantCulture, arguments);
}
Date._parse = function Date$_parse(value, cultureInfo, args) {
    var i, l, date, format, formats, custom = false;
    for (i = 1, l = args.length; i < l; i++) {
        format = args[i];
        if (format) {
            custom = true;
            date = Date._parseExact(value, format, cultureInfo);
            if (date) return date;
        }
    }
    if (! custom) {
        formats = cultureInfo._getDateTimeFormats();
        for (i = 0, l = formats.length; i < l; i++) {
            date = Date._parseExact(value, formats[i], cultureInfo);
            if (date) return date;
        }
    }
    return null;
}
Date._parseExact = function Date$_parseExact(value, format, cultureInfo) {
    value = value.trim();
    var dtf = cultureInfo.dateTimeFormat,
        parseInfo = Date._getParseRegExp(dtf, format),
        match = new RegExp(parseInfo.regExp).exec(value);
    if (match === null) return null;
    
    var groups = parseInfo.groups,
        era = null, year = null, month = null, date = null, weekDay = null,
        hour = 0, hourOffset, min = 0, sec = 0, msec = 0, tzMinOffset = null,
        pmHour = false;
    for (var j = 0, jl = groups.length; j < jl; j++) {
        var matchGroup = match[j+1];
        if (matchGroup) {
            switch (groups[j]) {
                case 'dd': case 'd':
                    date = parseInt(matchGroup, 10);
                    if ((date < 1) || (date > 31)) return null;
                    break;
                case 'MMMM':
                    month = cultureInfo._getMonthIndex(matchGroup);
                    if ((month < 0) || (month > 11)) return null;
                    break;
                case 'MMM':
                    month = cultureInfo._getAbbrMonthIndex(matchGroup);
                    if ((month < 0) || (month > 11)) return null;
                    break;
                case 'M': case 'MM':
                    month = parseInt(matchGroup, 10) - 1;
                    if ((month < 0) || (month > 11)) return null;
                    break;
                case 'y': case 'yy':
                    year = Date._expandYear(dtf,parseInt(matchGroup, 10));
                    if ((year < 0) || (year > 9999)) return null;
                    break;
                case 'yyyy':
                    year = parseInt(matchGroup, 10);
                    if ((year < 0) || (year > 9999)) return null;
                    break;
                case 'h': case 'hh':
                    hour = parseInt(matchGroup, 10);
                    if (hour === 12) hour = 0;
                    if ((hour < 0) || (hour > 11)) return null;
                    break;
                case 'H': case 'HH':
                    hour = parseInt(matchGroup, 10);
                    if ((hour < 0) || (hour > 23)) return null;
                    break;
                case 'm': case 'mm':
                    min = parseInt(matchGroup, 10);
                    if ((min < 0) || (min > 59)) return null;
                    break;
                case 's': case 'ss':
                    sec = parseInt(matchGroup, 10);
                    if ((sec < 0) || (sec > 59)) return null;
                    break;
                case 'tt': case 't':
                    var upperToken = matchGroup.toUpperCase();
                    pmHour = (upperToken === dtf.PMDesignator.toUpperCase());
                    if (!pmHour && (upperToken !== dtf.AMDesignator.toUpperCase())) return null;
                    break;
                case 'f':
                    msec = parseInt(matchGroup, 10) * 100;
                    if ((msec < 0) || (msec > 999)) return null;
                    break;
                case 'ff':
                    msec = parseInt(matchGroup, 10) * 10;
                    if ((msec < 0) || (msec > 999)) return null;
                    break;
                case 'fff':
                    msec = parseInt(matchGroup, 10);
                    if ((msec < 0) || (msec > 999)) return null;
                    break;
                case 'dddd':
                    weekDay = cultureInfo._getDayIndex(matchGroup);
                    if ((weekDay < 0) || (weekDay > 6)) return null;
                    break;
                case 'ddd':
                    weekDay = cultureInfo._getAbbrDayIndex(matchGroup);
                    if ((weekDay < 0) || (weekDay > 6)) return null;
                    break;
                case 'zzz':
                    var offsets = matchGroup.split(/:/);
                    if (offsets.length !== 2) return null;
                    hourOffset = parseInt(offsets[0], 10);
                    if ((hourOffset < -12) || (hourOffset > 13)) return null;
                    var minOffset = parseInt(offsets[1], 10);
                    if ((minOffset < 0) || (minOffset > 59)) return null;
                    tzMinOffset = (hourOffset * 60) + (matchGroup.startsWith('-')? -minOffset : minOffset);
                    break;
                case 'z': case 'zz':
                    hourOffset = parseInt(matchGroup, 10);
                    if ((hourOffset < -12) || (hourOffset > 13)) return null;
                    tzMinOffset = hourOffset * 60;
                    break;
                case 'g': case 'gg':
                    var eraName = matchGroup;
                    if (!eraName || !dtf.eras) return null;
                    eraName = eraName.toLowerCase().trim();
                    for (var i = 0, l = dtf.eras.length; i < l; i += 4) {
                        if (eraName === dtf.eras[i + 1].toLowerCase()) {
                            era = i;
                            break;
                        }
                    }
                    if (era === null) return null;
                    break;
            }
        }
    }
    var result = new Date(), defaults, convert = dtf.Calendar.convert;
    if (convert) {
        defaults = convert.fromGregorian(result);
    }
    if (!convert) {
        defaults = [result.getFullYear(), result.getMonth(), result.getDate()];
    }
    if (year === null) {
        year = defaults[0];
    }
    else if (dtf.eras) {
        year += dtf.eras[(era || 0) + 3];
    }
    if (month === null) {
        month = defaults[1];
    }
    if (date === null) {
        date = defaults[2];
    }
    if (convert) {
        result = convert.toGregorian(year, month, date);
        if (result === null) return null;
    }
    else {
        result.setFullYear(year, month, date);
        if (result.getDate() !== date) return null;
        if ((weekDay !== null) && (result.getDay() !== weekDay)) {
            return null;
        }
    }
    if (pmHour && (hour < 12)) {
        hour += 12;
    }
    result.setHours(hour, min, sec, msec);
    if (tzMinOffset !== null) {
        var adjustedMin = result.getMinutes() - (tzMinOffset + result.getTimezoneOffset());
        result.setHours(result.getHours() + parseInt(adjustedMin/60, 10), adjustedMin%60);
    }
    return result;
}
Date.prototype.format = function Date$format(format) {
    /// <summary locid="M:J#Date.format" />
    /// <param name="format" type="String"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String}
    ]);
    if (e) throw e;
    return this._toFormattedString(format, Sys.CultureInfo.InvariantCulture);
}
Date.prototype.localeFormat = function Date$localeFormat(format) {
    /// <summary locid="M:J#Date.localeFormat" />
    /// <param name="format" type="String"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String}
    ]);
    if (e) throw e;
    return this._toFormattedString(format, Sys.CultureInfo.CurrentCulture);
}
Date.prototype._toFormattedString = function Date$_toFormattedString(format, cultureInfo) {
    var dtf = cultureInfo.dateTimeFormat,
        convert = dtf.Calendar.convert;
    if (!format || !format.length || (format === 'i')) {
        if (cultureInfo && cultureInfo.name.length) {
            if (convert) {
                return this._toFormattedString(dtf.FullDateTimePattern, cultureInfo);
            }
            else {
                var eraDate = new Date(this.getTime());
                var era = Date._getEra(this, dtf.eras);
                eraDate.setFullYear(Date._getEraYear(this, dtf, era));
                return eraDate.toLocaleString();
            }
        }
        else {
            return this.toString();
        }
    }
    var eras = dtf.eras,
        sortable = (format === "s");
    format = Date._expandFormat(dtf, format);
    var ret = new Sys.StringBuilder();
    var hour;
    function addLeadingZero(num) {
        if (num < 10) {
            return '0' + num;
        }
        return num.toString();
    }
    function addLeadingZeros(num) {
        if (num < 10) {
            return '00' + num;
        }
        if (num < 100) {
            return '0' + num;
        }
        return num.toString();
    }
    function padYear(year) {
        if (year < 10) {
            return '000' + year;
        }
        else if (year < 100) {
            return '00' + year;
        }
        else if (year < 1000) {
            return '0' + year;
        }
        return year.toString();
    }
    
    var foundDay, checkedDay, dayPartRegExp = /([^d]|^)(d|dd)([^d]|$)/g;
    function hasDay() {
        if (foundDay || checkedDay) {
            return foundDay;
        }
        foundDay = dayPartRegExp.test(format);
        checkedDay = true;
        return foundDay;
    }
    
    var quoteCount = 0,
        tokenRegExp = Date._getTokenRegExp(),
        converted;
    if (!sortable && convert) {
        converted = convert.fromGregorian(this);
    }
    for (;;) {
        var index = tokenRegExp.lastIndex;
        var ar = tokenRegExp.exec(format);
        var preMatch = format.slice(index, ar ? ar.index : format.length);
        quoteCount += Date._appendPreOrPostMatch(preMatch, ret);
        if (!ar) break;
        if ((quoteCount%2) === 1) {
            ret.append(ar[0]);
            continue;
        }
        
        function getPart(date, part) {
            if (converted) {
                return converted[part];
            }
            switch (part) {
                case 0: return date.getFullYear();
                case 1: return date.getMonth();
                case 2: return date.getDate();
            }
        }
        switch (ar[0]) {
        case "dddd":
            ret.append(dtf.DayNames[this.getDay()]);
            break;
        case "ddd":
            ret.append(dtf.AbbreviatedDayNames[this.getDay()]);
            break;
        case "dd":
            foundDay = true;
            ret.append(addLeadingZero(getPart(this, 2)));
            break;
        case "d":
            foundDay = true;
            ret.append(getPart(this, 2));
            break;
        case "MMMM":
            ret.append((dtf.MonthGenitiveNames && hasDay())
                ? dtf.MonthGenitiveNames[getPart(this, 1)]
                : dtf.MonthNames[getPart(this, 1)]);
            break;
        case "MMM":
            ret.append((dtf.AbbreviatedMonthGenitiveNames && hasDay())
                ? dtf.AbbreviatedMonthGenitiveNames[getPart(this, 1)]
                : dtf.AbbreviatedMonthNames[getPart(this, 1)]);
            break;
        case "MM":
            ret.append(addLeadingZero(getPart(this, 1) + 1));
            break;
        case "M":
            ret.append(getPart(this, 1) + 1);
            break;
        case "yyyy":
            ret.append(padYear(converted ? converted[0] : Date._getEraYear(this, dtf, Date._getEra(this, eras), sortable)));
            break;
        case "yy":
            ret.append(addLeadingZero((converted ? converted[0] : Date._getEraYear(this, dtf, Date._getEra(this, eras), sortable)) % 100));
            break;
        case "y":
            ret.append((converted ? converted[0] : Date._getEraYear(this, dtf, Date._getEra(this, eras), sortable)) % 100);
            break;
        case "hh":
            hour = this.getHours() % 12;
            if (hour === 0) hour = 12;
            ret.append(addLeadingZero(hour));
            break;
        case "h":
            hour = this.getHours() % 12;
            if (hour === 0) hour = 12;
            ret.append(hour);
            break;
        case "HH":
            ret.append(addLeadingZero(this.getHours()));
            break;
        case "H":
            ret.append(this.getHours());
            break;
        case "mm":
            ret.append(addLeadingZero(this.getMinutes()));
            break;
        case "m":
            ret.append(this.getMinutes());
            break;
        case "ss":
            ret.append(addLeadingZero(this.getSeconds()));
            break;
        case "s":
            ret.append(this.getSeconds());
            break;
        case "tt":
            ret.append((this.getHours() < 12) ? dtf.AMDesignator : dtf.PMDesignator);
            break;
        case "t":
            ret.append(((this.getHours() < 12) ? dtf.AMDesignator : dtf.PMDesignator).charAt(0));
            break;
        case "f":
            ret.append(addLeadingZeros(this.getMilliseconds()).charAt(0));
            break;
        case "ff":
            ret.append(addLeadingZeros(this.getMilliseconds()).substr(0, 2));
            break;
        case "fff":
            ret.append(addLeadingZeros(this.getMilliseconds()));
            break;
        case "z":
            hour = this.getTimezoneOffset() / 60;
            ret.append(((hour <= 0) ? '+' : '-') + Math.floor(Math.abs(hour)));
            break;
        case "zz":
            hour = this.getTimezoneOffset() / 60;
            ret.append(((hour <= 0) ? '+' : '-') + addLeadingZero(Math.floor(Math.abs(hour))));
            break;
        case "zzz":
            hour = this.getTimezoneOffset() / 60;
            ret.append(((hour <= 0) ? '+' : '-') + addLeadingZero(Math.floor(Math.abs(hour))) +
                ":" + addLeadingZero(Math.abs(this.getTimezoneOffset() % 60)));
            break;
        case "g":
        case "gg":
            if (dtf.eras) {
                ret.append(dtf.eras[Date._getEra(this, eras) + 1]);
            }
            break;
        case "/":
            ret.append(dtf.DateSeparator);
            break;
        default:
            Sys.Debug.fail("Invalid date format pattern");
        }
    }
    return ret.toString();
}
String.localeFormat = function String$localeFormat(format, args) {
    /// <summary locid="M:J#String.localeFormat" />
    /// <param name="format" type="String"></param>
    /// <param name="args" parameterArray="true" mayBeNull="true"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String},
        {name: "args", mayBeNull: true, parameterArray: true}
    ]);
    if (e) throw e;
    return String._toFormattedString(true, arguments);
}
Number.parseLocale = function Number$parseLocale(value) {
    /// <summary locid="M:J#Number.parseLocale" />
    /// <param name="value" type="String"></param>
    /// <returns type="Number"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String}
    ], false);
    if (e) throw e;
    return Number._parse(value, Sys.CultureInfo.CurrentCulture);
}
Number.parseInvariant = function Number$parseInvariant(value) {
    /// <summary locid="M:J#Number.parseInvariant" />
    /// <param name="value" type="String"></param>
    /// <returns type="Number"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String}
    ], false);
    if (e) throw e;
    return Number._parse(value, Sys.CultureInfo.InvariantCulture);
}
Number._parse = function Number$_parse(value, cultureInfo) {
    value = value.trim();
    
    if (value.match(/^[+-]?infinity$/i)) {
        return parseFloat(value);
    }
    if (value.match(/^0x[a-f0-9]+$/i)) {
        return parseInt(value);
    }
    var numFormat = cultureInfo.numberFormat;
    var signInfo = Number._parseNumberNegativePattern(value, numFormat, numFormat.NumberNegativePattern);
    var sign = signInfo[0];
    var num = signInfo[1];
    
    if ((sign === '') && (numFormat.NumberNegativePattern !== 1)) {
        signInfo = Number._parseNumberNegativePattern(value, numFormat, 1);
        sign = signInfo[0];
        num = signInfo[1];
    }
    if (sign === '') sign = '+';
    
    var exponent;
    var intAndFraction;
    var exponentPos = num.indexOf('e');
    if (exponentPos < 0) exponentPos = num.indexOf('E');
    if (exponentPos < 0) {
        intAndFraction = num;
        exponent = null;
    }
    else {
        intAndFraction = num.substr(0, exponentPos);
        exponent = num.substr(exponentPos + 1);
    }
    
    var integer;
    var fraction;
    var decimalPos = intAndFraction.indexOf(numFormat.NumberDecimalSeparator);
    if (decimalPos < 0) {
        integer = intAndFraction;
        fraction = null;
    }
    else {
        integer = intAndFraction.substr(0, decimalPos);
        fraction = intAndFraction.substr(decimalPos + numFormat.NumberDecimalSeparator.length);
    }
    
    integer = integer.split(numFormat.NumberGroupSeparator).join('');
    var altNumGroupSeparator = numFormat.NumberGroupSeparator.replace(/\u00A0/g, " ");
    if (numFormat.NumberGroupSeparator !== altNumGroupSeparator) {
        integer = integer.split(altNumGroupSeparator).join('');
    }
    
    var p = sign + integer;
    if (fraction !== null) {
        p += '.' + fraction;
    }
    if (exponent !== null) {
        var expSignInfo = Number._parseNumberNegativePattern(exponent, numFormat, 1);
        if (expSignInfo[0] === '') {
            expSignInfo[0] = '+';
        }
        p += 'e' + expSignInfo[0] + expSignInfo[1];
    }
    if (p.match(/^[+-]?\d*\.?\d*(e[+-]?\d+)?$/)) {
        return parseFloat(p);
    }
    return Number.NaN;
}
Number._parseNumberNegativePattern = function Number$_parseNumberNegativePattern(value, numFormat, numberNegativePattern) {
    var neg = numFormat.NegativeSign;
    var pos = numFormat.PositiveSign;    
    switch (numberNegativePattern) {
        case 4: 
            neg = ' ' + neg;
            pos = ' ' + pos;
        case 3: 
            if (value.endsWith(neg)) {
                return ['-', value.substr(0, value.length - neg.length)];
            }
            else if (value.endsWith(pos)) {
                return ['+', value.substr(0, value.length - pos.length)];
            }
            break;
        case 2: 
            neg += ' ';
            pos += ' ';
        case 1: 
            if (value.startsWith(neg)) {
                return ['-', value.substr(neg.length)];
            }
            else if (value.startsWith(pos)) {
                return ['+', value.substr(pos.length)];
            }
            break;
        case 0: 
            if (value.startsWith('(') && value.endsWith(')')) {
                return ['-', value.substr(1, value.length - 2)];
            }
            break;
        default:
            Sys.Debug.fail("");
    }
    return ['', value];
}
Number.prototype.format = function Number$format(format) {
    /// <summary locid="M:J#Number.format" />
    /// <param name="format" type="String"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String}
    ]);
    if (e) throw e;
    return this._toFormattedString(format, Sys.CultureInfo.InvariantCulture);
}
Number.prototype.localeFormat = function Number$localeFormat(format) {
    /// <summary locid="M:J#Number.localeFormat" />
    /// <param name="format" type="String"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "format", type: String}
    ]);
    if (e) throw e;
    return this._toFormattedString(format, Sys.CultureInfo.CurrentCulture);
}
Number.prototype._toFormattedString = function Number$_toFormattedString(format, cultureInfo) {
    if (!format || (format.length === 0) || (format === 'i')) {
        if (cultureInfo && (cultureInfo.name.length > 0)) {
            return this.toLocaleString();
        }
        else {
            return this.toString();
        }
    }
    
    var _percentPositivePattern = ["n %", "n%", "%n" ];
    var _percentNegativePattern = ["-n %", "-n%", "-%n"];
    var _numberNegativePattern = ["(n)","-n","- n","n-","n -"];
    var _currencyPositivePattern = ["$n","n$","$ n","n $"];
    var _currencyNegativePattern = ["($n)","-$n","$-n","$n-","(n$)","-n$","n-$","n$-","-n $","-$ n","n $-","$ n-","$ -n","n- $","($ n)","(n $)"];
    function zeroPad(str, count, left) {
        for (var l=str.length; l < count; l++) {
            str = (left ? ('0' + str) : (str + '0'));
        }
        return str;
    }
    
    function expandNumber(number, precision, groupSizes, sep, decimalChar) {
        Sys.Debug.assert(groupSizes.length > 0, "groupSizes must be an array of at least 1");
        var curSize = groupSizes[0];
        var curGroupIndex = 1;
        var factor = Math.pow(10, precision);
        var rounded = (Math.round(number * factor) / factor);
        if (!isFinite(rounded)) {
            rounded = number;
        }
        number = rounded;
        
        var numberString = number.toString();
        var right = "";
        var exponent;
        
        
        var split = numberString.split(/e/i);
        numberString = split[0];
        exponent = (split.length > 1 ? parseInt(split[1]) : 0);
        split = numberString.split('.');
        numberString = split[0];
        right = split.length > 1 ? split[1] : "";
        
        var l;
        if (exponent > 0) {
            right = zeroPad(right, exponent, false);
            numberString += right.slice(0, exponent);
            right = right.substr(exponent);
        }
        else if (exponent < 0) {
            exponent = -exponent;
            numberString = zeroPad(numberString, exponent+1, true);
            right = numberString.slice(-exponent, numberString.length) + right;
            numberString = numberString.slice(0, -exponent);
        }
        if (precision > 0) {
            if (right.length > precision) {
                right = right.slice(0, precision);
            }
            else {
                right = zeroPad(right, precision, false);
            }
            right = decimalChar + right;
        }
        else { 
            right = "";
        }
        var stringIndex = numberString.length-1;
        var ret = "";
        while (stringIndex >= 0) {
            if (curSize === 0 || curSize > stringIndex) {
                if (ret.length > 0)
                    return numberString.slice(0, stringIndex + 1) + sep + ret + right;
                else
                    return numberString.slice(0, stringIndex + 1) + right;
            }
            if (ret.length > 0)
                ret = numberString.slice(stringIndex - curSize + 1, stringIndex+1) + sep + ret;
            else
                ret = numberString.slice(stringIndex - curSize + 1, stringIndex+1);
            stringIndex -= curSize;
            if (curGroupIndex < groupSizes.length) {
                curSize = groupSizes[curGroupIndex];
                curGroupIndex++;
            }
        }
        return numberString.slice(0, stringIndex + 1) + sep + ret + right;
    }
    var nf = cultureInfo.numberFormat;
    var number = Math.abs(this);
    if (!format)
        format = "D";
    var precision = -1;
    if (format.length > 1) precision = parseInt(format.slice(1), 10);
    var pattern;
    switch (format.charAt(0)) {
    case "d":
    case "D":
        pattern = 'n';
        if (precision !== -1) {
            number = zeroPad(""+number, precision, true);
        }
        if (this < 0) number = -number;
        break;
    case "c":
    case "C":
        if (this < 0) pattern = _currencyNegativePattern[nf.CurrencyNegativePattern];
        else pattern = _currencyPositivePattern[nf.CurrencyPositivePattern];
        if (precision === -1) precision = nf.CurrencyDecimalDigits;
        number = expandNumber(Math.abs(this), precision, nf.CurrencyGroupSizes, nf.CurrencyGroupSeparator, nf.CurrencyDecimalSeparator);
        break;
    case "n":
    case "N":
        if (this < 0) pattern = _numberNegativePattern[nf.NumberNegativePattern];
        else pattern = 'n';
        if (precision === -1) precision = nf.NumberDecimalDigits;
        number = expandNumber(Math.abs(this), precision, nf.NumberGroupSizes, nf.NumberGroupSeparator, nf.NumberDecimalSeparator);
        break;
    case "p":
    case "P":
        if (this < 0) pattern = _percentNegativePattern[nf.PercentNegativePattern];
        else pattern = _percentPositivePattern[nf.PercentPositivePattern];
        if (precision === -1) precision = nf.PercentDecimalDigits;
        number = expandNumber(Math.abs(this) * 100, precision, nf.PercentGroupSizes, nf.PercentGroupSeparator, nf.PercentDecimalSeparator);
        break;
    default:
        throw Error.format(Sys.Res.formatBadFormatSpecifier);
    }
    var regex = /n|\$|-|%/g;
    var ret = "";
    for (;;) {
        var index = regex.lastIndex;
        var ar = regex.exec(pattern);
        ret += pattern.slice(index, ar ? ar.index : pattern.length);
        if (!ar)
            break;
        switch (ar[0]) {
        case "n":
            ret += number;
            break;
        case "$":
            ret += nf.CurrencySymbol;
            break;
        case "-":
            if (/[1-9]/.test(number)) {
                ret += nf.NegativeSign;
            }
            break;
        case "%":
            ret += nf.PercentSymbol;
            break;
        default:
            Sys.Debug.fail("Invalid number format pattern");
        }
    }
    return ret;
}
 
Sys.CultureInfo = function Sys$CultureInfo(name, numberFormat, dateTimeFormat) {
    /// <summary locid="M:J#Sys.CultureInfo.#ctor" />
    /// <param name="name" type="String"></param>
    /// <param name="numberFormat" type="Object"></param>
    /// <param name="dateTimeFormat" type="Object"></param>
    var e = Function._validateParams(arguments, [
        {name: "name", type: String},
        {name: "numberFormat", type: Object},
        {name: "dateTimeFormat", type: Object}
    ]);
    if (e) throw e;
    this.name = name;
    this.numberFormat = numberFormat;
    this.dateTimeFormat = dateTimeFormat;
}
    function Sys$CultureInfo$_getDateTimeFormats() {
        if (! this._dateTimeFormats) {
            var dtf = this.dateTimeFormat;
            this._dateTimeFormats =
              [ dtf.MonthDayPattern,
                dtf.YearMonthPattern,
                dtf.ShortDatePattern,
                dtf.ShortTimePattern,
                dtf.LongDatePattern,
                dtf.LongTimePattern,
                dtf.FullDateTimePattern,
                dtf.RFC1123Pattern,
                dtf.SortableDateTimePattern,
                dtf.UniversalSortableDateTimePattern ];
        }
        return this._dateTimeFormats;
    }
    function Sys$CultureInfo$_getIndex(value, a1, a2) {
        var upper = this._toUpper(value),
            i = Array.indexOf(a1, upper);
        if (i === -1) {
            i = Array.indexOf(a2, upper);
        }
        return i;
    }
    function Sys$CultureInfo$_getMonthIndex(value) {
        if (!this._upperMonths) {
            this._upperMonths = this._toUpperArray(this.dateTimeFormat.MonthNames);
            this._upperMonthsGenitive = this._toUpperArray(this.dateTimeFormat.MonthGenitiveNames);
        }
        return this._getIndex(value, this._upperMonths, this._upperMonthsGenitive);
    }
    function Sys$CultureInfo$_getAbbrMonthIndex(value) {
        if (!this._upperAbbrMonths) {
            this._upperAbbrMonths = this._toUpperArray(this.dateTimeFormat.AbbreviatedMonthNames);
            this._upperAbbrMonthsGenitive = this._toUpperArray(this.dateTimeFormat.AbbreviatedMonthGenitiveNames);
        }
        return this._getIndex(value, this._upperAbbrMonths, this._upperAbbrMonthsGenitive);
    }
    function Sys$CultureInfo$_getDayIndex(value) {
        if (!this._upperDays) {
            this._upperDays = this._toUpperArray(this.dateTimeFormat.DayNames);
        }
        return Array.indexOf(this._upperDays, this._toUpper(value));
    }
    function Sys$CultureInfo$_getAbbrDayIndex(value) {
        if (!this._upperAbbrDays) {
            this._upperAbbrDays = this._toUpperArray(this.dateTimeFormat.AbbreviatedDayNames);
        }
        return Array.indexOf(this._upperAbbrDays, this._toUpper(value));
    }
    function Sys$CultureInfo$_toUpperArray(arr) {
        var result = [];
        for (var i = 0, il = arr.length; i < il; i++) {
            result[i] = this._toUpper(arr[i]);
        }
        return result;
    }
    function Sys$CultureInfo$_toUpper(value) {
        return value.split("\u00A0").join(' ').toUpperCase();
    }
Sys.CultureInfo.prototype = {
    _getDateTimeFormats: Sys$CultureInfo$_getDateTimeFormats,
    _getIndex: Sys$CultureInfo$_getIndex,
    _getMonthIndex: Sys$CultureInfo$_getMonthIndex,
    _getAbbrMonthIndex: Sys$CultureInfo$_getAbbrMonthIndex,
    _getDayIndex: Sys$CultureInfo$_getDayIndex,
    _getAbbrDayIndex: Sys$CultureInfo$_getAbbrDayIndex,
    _toUpperArray: Sys$CultureInfo$_toUpperArray,
    _toUpper: Sys$CultureInfo$_toUpper
}
Sys.CultureInfo.registerClass('Sys.CultureInfo');
Sys.CultureInfo._parse = function Sys$CultureInfo$_parse(value) {
    var dtf = value.dateTimeFormat;
    if (dtf && !dtf.eras) {
        dtf.eras = value.eras;
    }
    return new Sys.CultureInfo(value.name, value.numberFormat, dtf);
}
Sys.CultureInfo.InvariantCulture = Sys.CultureInfo._parse({"name":"","numberFormat":{"CurrencyDecimalDigits":2,"CurrencyDecimalSeparator":".","IsReadOnly":true,"CurrencyGroupSizes":[3],"NumberGroupSizes":[3],"PercentGroupSizes":[3],"CurrencyGroupSeparator":",","CurrencySymbol":"\u00A4","NaNSymbol":"NaN","CurrencyNegativePattern":0,"NumberNegativePattern":1,"PercentPositivePattern":0,"PercentNegativePattern":0,"NegativeInfinitySymbol":"-Infinity","NegativeSign":"-","NumberDecimalDigits":2,"NumberDecimalSeparator":".","NumberGroupSeparator":",","CurrencyPositivePattern":0,"PositiveInfinitySymbol":"Infinity","PositiveSign":"+","PercentDecimalDigits":2,"PercentDecimalSeparator":".","PercentGroupSeparator":",","PercentSymbol":"%","PerMilleSymbol":"\u2030","NativeDigits":["0","1","2","3","4","5","6","7","8","9"],"DigitSubstitution":1},"dateTimeFormat":{"AMDesignator":"AM","Calendar":{"MinSupportedDateTime":"@-62135568000000@","MaxSupportedDateTime":"@253402300799999@","AlgorithmType":1,"CalendarType":1,"Eras":[1],"TwoDigitYearMax":2029,"IsReadOnly":true},"DateSeparator":"/","FirstDayOfWeek":0,"CalendarWeekRule":0,"FullDateTimePattern":"dddd, dd MMMM yyyy HH:mm:ss","LongDatePattern":"dddd, dd MMMM yyyy","LongTimePattern":"HH:mm:ss","MonthDayPattern":"MMMM dd","PMDesignator":"PM","RFC1123Pattern":"ddd, dd MMM yyyy HH\':\'mm\':\'ss \'GMT\'","ShortDatePattern":"MM/dd/yyyy","ShortTimePattern":"HH:mm","SortableDateTimePattern":"yyyy\'-\'MM\'-\'dd\'T\'HH\':\'mm\':\'ss","TimeSeparator":":","UniversalSortableDateTimePattern":"yyyy\'-\'MM\'-\'dd HH\':\'mm\':\'ss\'Z\'","YearMonthPattern":"yyyy MMMM","AbbreviatedDayNames":["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],"ShortestDayNames":["Su","Mo","Tu","We","Th","Fr","Sa"],"DayNames":["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],"AbbreviatedMonthNames":["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec",""],"MonthNames":["January","February","March","April","May","June","July","August","September","October","November","December",""],"IsReadOnly":true,"NativeCalendarName":"Gregorian Calendar","AbbreviatedMonthGenitiveNames":["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec",""],"MonthGenitiveNames":["January","February","March","April","May","June","July","August","September","October","November","December",""]},"eras":[1,"A.D.",null,0]});
if (typeof(__cultureInfo) === "object") {
    Sys.CultureInfo.CurrentCulture = Sys.CultureInfo._parse(__cultureInfo);
    delete __cultureInfo;    
}
else {
    Sys.CultureInfo.CurrentCulture = Sys.CultureInfo._parse({"name":"en-US","numberFormat":{"CurrencyDecimalDigits":2,"CurrencyDecimalSeparator":".","IsReadOnly":false,"CurrencyGroupSizes":[3],"NumberGroupSizes":[3],"PercentGroupSizes":[3],"CurrencyGroupSeparator":",","CurrencySymbol":"$","NaNSymbol":"NaN","CurrencyNegativePattern":0,"NumberNegativePattern":1,"PercentPositivePattern":0,"PercentNegativePattern":0,"NegativeInfinitySymbol":"-Infinity","NegativeSign":"-","NumberDecimalDigits":2,"NumberDecimalSeparator":".","NumberGroupSeparator":",","CurrencyPositivePattern":0,"PositiveInfinitySymbol":"Infinity","PositiveSign":"+","PercentDecimalDigits":2,"PercentDecimalSeparator":".","PercentGroupSeparator":",","PercentSymbol":"%","PerMilleSymbol":"\u2030","NativeDigits":["0","1","2","3","4","5","6","7","8","9"],"DigitSubstitution":1},"dateTimeFormat":{"AMDesignator":"AM","Calendar":{"MinSupportedDateTime":"@-62135568000000@","MaxSupportedDateTime":"@253402300799999@","AlgorithmType":1,"CalendarType":1,"Eras":[1],"TwoDigitYearMax":2029,"IsReadOnly":false},"DateSeparator":"/","FirstDayOfWeek":0,"CalendarWeekRule":0,"FullDateTimePattern":"dddd, MMMM dd, yyyy h:mm:ss tt","LongDatePattern":"dddd, MMMM dd, yyyy","LongTimePattern":"h:mm:ss tt","MonthDayPattern":"MMMM dd","PMDesignator":"PM","RFC1123Pattern":"ddd, dd MMM yyyy HH\':\'mm\':\'ss \'GMT\'","ShortDatePattern":"M/d/yyyy","ShortTimePattern":"h:mm tt","SortableDateTimePattern":"yyyy\'-\'MM\'-\'dd\'T\'HH\':\'mm\':\'ss","TimeSeparator":":","UniversalSortableDateTimePattern":"yyyy\'-\'MM\'-\'dd HH\':\'mm\':\'ss\'Z\'","YearMonthPattern":"MMMM, yyyy","AbbreviatedDayNames":["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],"ShortestDayNames":["Su","Mo","Tu","We","Th","Fr","Sa"],"DayNames":["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],"AbbreviatedMonthNames":["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec",""],"MonthNames":["January","February","March","April","May","June","July","August","September","October","November","December",""],"IsReadOnly":false,"NativeCalendarName":"Gregorian Calendar","AbbreviatedMonthGenitiveNames":["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec",""],"MonthGenitiveNames":["January","February","March","April","May","June","July","August","September","October","November","December",""]},"eras":[1,"A.D.",null,0]});
}
Type.registerNamespace('Sys.Serialization');
Sys.Serialization.JavaScriptSerializer = function Sys$Serialization$JavaScriptSerializer() {
    /// <summary locid="M:J#Sys.Serialization.JavaScriptSerializer.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
}
Sys.Serialization.JavaScriptSerializer.registerClass('Sys.Serialization.JavaScriptSerializer');
Sys.Serialization.JavaScriptSerializer._charsToEscapeRegExs = [];
Sys.Serialization.JavaScriptSerializer._charsToEscape = [];
Sys.Serialization.JavaScriptSerializer._dateRegEx = new RegExp('(^|[^\\\\])\\"\\\\/Date\\((-?[0-9]+)(?:[a-zA-Z]|(?:\\+|-)[0-9]{4})?\\)\\\\/\\"', 'g');
Sys.Serialization.JavaScriptSerializer._escapeChars = {};
Sys.Serialization.JavaScriptSerializer._escapeRegEx = new RegExp('["\\\\\\x00-\\x1F]', 'i');
Sys.Serialization.JavaScriptSerializer._escapeRegExGlobal = new RegExp('["\\\\\\x00-\\x1F]', 'g');
Sys.Serialization.JavaScriptSerializer._jsonRegEx = new RegExp('[^,:{}\\[\\]0-9.\\-+Eaeflnr-u \\n\\r\\t]', 'g');
Sys.Serialization.JavaScriptSerializer._jsonStringRegEx = new RegExp('"(\\\\.|[^"\\\\])*"', 'g');
Sys.Serialization.JavaScriptSerializer._serverTypeFieldName = '__type';
Sys.Serialization.JavaScriptSerializer._init = function Sys$Serialization$JavaScriptSerializer$_init() {
    var replaceChars = ['\\u0000','\\u0001','\\u0002','\\u0003','\\u0004','\\u0005','\\u0006','\\u0007',
                        '\\b','\\t','\\n','\\u000b','\\f','\\r','\\u000e','\\u000f','\\u0010','\\u0011',
                        '\\u0012','\\u0013','\\u0014','\\u0015','\\u0016','\\u0017','\\u0018','\\u0019',
                        '\\u001a','\\u001b','\\u001c','\\u001d','\\u001e','\\u001f'];
    Sys.Serialization.JavaScriptSerializer._charsToEscape[0] = '\\';
    Sys.Serialization.JavaScriptSerializer._charsToEscapeRegExs['\\'] = new RegExp('\\\\', 'g');
    Sys.Serialization.JavaScriptSerializer._escapeChars['\\'] = '\\\\';
    Sys.Serialization.JavaScriptSerializer._charsToEscape[1] = '"';
    Sys.Serialization.JavaScriptSerializer._charsToEscapeRegExs['"'] = new RegExp('"', 'g');
    Sys.Serialization.JavaScriptSerializer._escapeChars['"'] = '\\"';
    for (var i = 0; i < 32; i++) {
        var c = String.fromCharCode(i);
        Sys.Serialization.JavaScriptSerializer._charsToEscape[i+2] = c;
        Sys.Serialization.JavaScriptSerializer._charsToEscapeRegExs[c] = new RegExp(c, 'g');
        Sys.Serialization.JavaScriptSerializer._escapeChars[c] = replaceChars[i];
    }
}
Sys.Serialization.JavaScriptSerializer._serializeBooleanWithBuilder = function Sys$Serialization$JavaScriptSerializer$_serializeBooleanWithBuilder(object, stringBuilder) {
    stringBuilder.append(object.toString());
}
Sys.Serialization.JavaScriptSerializer._serializeNumberWithBuilder = function Sys$Serialization$JavaScriptSerializer$_serializeNumberWithBuilder(object, stringBuilder) {
    if (isFinite(object)) {
        stringBuilder.append(String(object));
    }
    else {
        throw Error.invalidOperation(Sys.Res.cannotSerializeNonFiniteNumbers);
    }
}
Sys.Serialization.JavaScriptSerializer._serializeStringWithBuilder = function Sys$Serialization$JavaScriptSerializer$_serializeStringWithBuilder(string, stringBuilder) {
    stringBuilder.append('"');
    if (Sys.Serialization.JavaScriptSerializer._escapeRegEx.test(string)) {
        if (Sys.Serialization.JavaScriptSerializer._charsToEscape.length === 0) {
            Sys.Serialization.JavaScriptSerializer._init();
        }
        if (string.length < 128) {
            string = string.replace(Sys.Serialization.JavaScriptSerializer._escapeRegExGlobal,
                function(x) { return Sys.Serialization.JavaScriptSerializer._escapeChars[x]; });
        }
        else {
            for (var i = 0; i < 34; i++) {
                var c = Sys.Serialization.JavaScriptSerializer._charsToEscape[i];
                if (string.indexOf(c) !== -1) {
                    if (Sys.Browser.agent === Sys.Browser.Opera || Sys.Browser.agent === Sys.Browser.FireFox) {
                        string = string.split(c).join(Sys.Serialization.JavaScriptSerializer._escapeChars[c]);
                    }
                    else {
                        string = string.replace(Sys.Serialization.JavaScriptSerializer._charsToEscapeRegExs[c],
                            Sys.Serialization.JavaScriptSerializer._escapeChars[c]);
                    }
                }
            }
       }
    }
    stringBuilder.append(string);
    stringBuilder.append('"');
}
Sys.Serialization.JavaScriptSerializer._serializeWithBuilder = function Sys$Serialization$JavaScriptSerializer$_serializeWithBuilder(object, stringBuilder, sort, prevObjects) {
    var i;
    switch (typeof object) {
    case 'object':
        if (object) {
            if (prevObjects){
                for( var j = 0; j < prevObjects.length; j++) {
                    if (prevObjects[j] === object) {
                        throw Error.invalidOperation(Sys.Res.cannotSerializeObjectWithCycle);
                    }
                }
            }
            else {
                prevObjects = new Array();
            }
            try {
                Array.add(prevObjects, object);
                
                if (Number.isInstanceOfType(object)){
                    Sys.Serialization.JavaScriptSerializer._serializeNumberWithBuilder(object, stringBuilder);
                }
                else if (Boolean.isInstanceOfType(object)){
                    Sys.Serialization.JavaScriptSerializer._serializeBooleanWithBuilder(object, stringBuilder);
                }
                else if (String.isInstanceOfType(object)){
                    Sys.Serialization.JavaScriptSerializer._serializeStringWithBuilder(object, stringBuilder);
                }
            
                else if (Array.isInstanceOfType(object)) {
                    stringBuilder.append('[');
                   
                    for (i = 0; i < object.length; ++i) {
                        if (i > 0) {
                            stringBuilder.append(',');
                        }
                        Sys.Serialization.JavaScriptSerializer._serializeWithBuilder(object[i], stringBuilder,false,prevObjects);
                    }
                    stringBuilder.append(']');
                }
                else {
                    if (Date.isInstanceOfType(object)) {
                        stringBuilder.append('"\\/Date(');
                        stringBuilder.append(object.getTime());
                        stringBuilder.append(')\\/"');
                        break;
                    }
                    var properties = [];
                    var propertyCount = 0;
                    for (var name in object) {
                        if (name.startsWith('$')) {
                            continue;
                        }
                        if (name === Sys.Serialization.JavaScriptSerializer._serverTypeFieldName && propertyCount !== 0){
                            properties[propertyCount++] = properties[0];
                            properties[0] = name;
                        }
                        else{
                            properties[propertyCount++] = name;
                        }
                    }
                    if (sort) properties.sort();
                    stringBuilder.append('{');
                    var needComma = false;
                     
                    for (i=0; i<propertyCount; i++) {
                        var value = object[properties[i]];
                        if (typeof value !== 'undefined' && typeof value !== 'function') {
                            if (needComma) {
                                stringBuilder.append(',');
                            }
                            else {
                                needComma = true;
                            }
                           
                            Sys.Serialization.JavaScriptSerializer._serializeWithBuilder(properties[i], stringBuilder, sort, prevObjects);
                            stringBuilder.append(':');
                            Sys.Serialization.JavaScriptSerializer._serializeWithBuilder(value, stringBuilder, sort, prevObjects);
                          
                        }
                    }
                stringBuilder.append('}');
                }
            }
            finally {
                Array.removeAt(prevObjects, prevObjects.length - 1);
            }
        }
        else {
            stringBuilder.append('null');
        }
        break;
    case 'number':
        Sys.Serialization.JavaScriptSerializer._serializeNumberWithBuilder(object, stringBuilder);
        break;
    case 'string':
        Sys.Serialization.JavaScriptSerializer._serializeStringWithBuilder(object, stringBuilder);
        break;
    case 'boolean':
        Sys.Serialization.JavaScriptSerializer._serializeBooleanWithBuilder(object, stringBuilder);
        break;
    default:
        stringBuilder.append('null');
        break;
    }
}
Sys.Serialization.JavaScriptSerializer.serialize = function Sys$Serialization$JavaScriptSerializer$serialize(object) {
    /// <summary locid="M:J#Sys.Serialization.JavaScriptSerializer.serialize" />
    /// <param name="object" mayBeNull="true"></param>
    /// <returns type="String"></returns>
    var e = Function._validateParams(arguments, [
        {name: "object", mayBeNull: true}
    ]);
    if (e) throw e;
    var stringBuilder = new Sys.StringBuilder();
    Sys.Serialization.JavaScriptSerializer._serializeWithBuilder(object, stringBuilder, false);
    return stringBuilder.toString();
}
Sys.Serialization.JavaScriptSerializer.deserialize = function Sys$Serialization$JavaScriptSerializer$deserialize(data, secure) {
    /// <summary locid="M:J#Sys.Serialization.JavaScriptSerializer.deserialize" />
    /// <param name="data" type="String"></param>
    /// <param name="secure" type="Boolean" optional="true"></param>
    /// <returns></returns>
    var e = Function._validateParams(arguments, [
        {name: "data", type: String},
        {name: "secure", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    
    if (data.length === 0) throw Error.argument('data', Sys.Res.cannotDeserializeEmptyString);
    try {    
        var exp = data.replace(Sys.Serialization.JavaScriptSerializer._dateRegEx, "$1new Date($2)");
        
        if (secure && Sys.Serialization.JavaScriptSerializer._jsonRegEx.test(
             exp.replace(Sys.Serialization.JavaScriptSerializer._jsonStringRegEx, ''))) throw null;
        return eval('(' + exp + ')');
    }
    catch (e) {
         throw Error.argument('data', Sys.Res.cannotDeserializeInvalidJson);
    }
}
Type.registerNamespace('Sys.UI');
 
Sys.EventHandlerList = function Sys$EventHandlerList() {
    /// <summary locid="M:J#Sys.EventHandlerList.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    this._list = {};
}
    function Sys$EventHandlerList$_addHandler(id, handler) {
        Array.add(this._getEvent(id, true), handler);
    }
    function Sys$EventHandlerList$addHandler(id, handler) {
        /// <summary locid="M:J#Sys.EventHandlerList.addHandler" />
        /// <param name="id" type="String"></param>
        /// <param name="handler" type="Function"></param>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String},
            {name: "handler", type: Function}
        ]);
        if (e) throw e;
        this._addHandler(id, handler);
    }
    function Sys$EventHandlerList$_removeHandler(id, handler) {
        var evt = this._getEvent(id);
        if (!evt) return;
        Array.remove(evt, handler);
    }
    function Sys$EventHandlerList$removeHandler(id, handler) {
        /// <summary locid="M:J#Sys.EventHandlerList.removeHandler" />
        /// <param name="id" type="String"></param>
        /// <param name="handler" type="Function"></param>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String},
            {name: "handler", type: Function}
        ]);
        if (e) throw e;
        this._removeHandler(id, handler);
    }
    function Sys$EventHandlerList$getHandler(id) {
        /// <summary locid="M:J#Sys.EventHandlerList.getHandler" />
        /// <param name="id" type="String"></param>
        /// <returns type="Function"></returns>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String}
        ]);
        if (e) throw e;
        var evt = this._getEvent(id);
        if (!evt || (evt.length === 0)) return null;
        evt = Array.clone(evt);
        return function(source, args) {
            for (var i = 0, l = evt.length; i < l; i++) {
                evt[i](source, args);
            }
        };
    }
    function Sys$EventHandlerList$_getEvent(id, create) {
        if (!this._list[id]) {
            if (!create) return null;
            this._list[id] = [];
        }
        return this._list[id];
    }
Sys.EventHandlerList.prototype = {
    _addHandler: Sys$EventHandlerList$_addHandler,
    addHandler: Sys$EventHandlerList$addHandler,
    _removeHandler: Sys$EventHandlerList$_removeHandler,
    removeHandler: Sys$EventHandlerList$removeHandler,
    getHandler: Sys$EventHandlerList$getHandler,
    _getEvent: Sys$EventHandlerList$_getEvent
}
Sys.EventHandlerList.registerClass('Sys.EventHandlerList');
Sys.CommandEventArgs = function Sys$CommandEventArgs(commandName, commandArgument, commandSource) {
    /// <summary locid="M:J#Sys.CommandEventArgs.#ctor" />
    /// <param name="commandName" type="String"></param>
    /// <param name="commandArgument" mayBeNull="true"></param>
    /// <param name="commandSource" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "commandName", type: String},
        {name: "commandArgument", mayBeNull: true},
        {name: "commandSource", mayBeNull: true}
    ]);
    if (e) throw e;
    Sys.CommandEventArgs.initializeBase(this);
    this._commandName = commandName;
    this._commandArgument = commandArgument;
    this._commandSource = commandSource;
}
    function Sys$CommandEventArgs$get_commandName() {
        /// <value type="String" locid="P:J#Sys.CommandEventArgs.commandName"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._commandName;
    }
    function Sys$CommandEventArgs$get_commandArgument() {
        /// <value mayBeNull="true" locid="P:J#Sys.CommandEventArgs.commandArgument"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._commandArgument;
    }
    function Sys$CommandEventArgs$get_commandSource() {
        /// <value mayBeNull="true" locid="P:J#Sys.CommandEventArgs.commandSource"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._commandSource;
    }
Sys.CommandEventArgs.prototype = {
    _commandName: null,
    _commandArgument: null,
    _commandSource: null,
    get_commandName: Sys$CommandEventArgs$get_commandName,
    get_commandArgument: Sys$CommandEventArgs$get_commandArgument,
    get_commandSource: Sys$CommandEventArgs$get_commandSource
}
Sys.CommandEventArgs.registerClass("Sys.CommandEventArgs", Sys.CancelEventArgs);
 
Sys.INotifyPropertyChange = function Sys$INotifyPropertyChange() {
    /// <summary locid="M:J#Sys.INotifyPropertyChange.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
    function Sys$INotifyPropertyChange$add_propertyChanged(handler) {
    /// <summary locid="E:J#Sys.INotifyPropertyChange.propertyChanged" />
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$INotifyPropertyChange$remove_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
Sys.INotifyPropertyChange.prototype = {
    add_propertyChanged: Sys$INotifyPropertyChange$add_propertyChanged,
    remove_propertyChanged: Sys$INotifyPropertyChange$remove_propertyChanged
}
Sys.INotifyPropertyChange.registerInterface('Sys.INotifyPropertyChange');
 
Sys.PropertyChangedEventArgs = function Sys$PropertyChangedEventArgs(propertyName) {
    /// <summary locid="M:J#Sys.PropertyChangedEventArgs.#ctor" />
    /// <param name="propertyName" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "propertyName", type: String}
    ]);
    if (e) throw e;
    Sys.PropertyChangedEventArgs.initializeBase(this);
    this._propertyName = propertyName;
}
 
    function Sys$PropertyChangedEventArgs$get_propertyName() {
        /// <value type="String" locid="P:J#Sys.PropertyChangedEventArgs.propertyName"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._propertyName;
    }
Sys.PropertyChangedEventArgs.prototype = {
    get_propertyName: Sys$PropertyChangedEventArgs$get_propertyName
}
Sys.PropertyChangedEventArgs.registerClass('Sys.PropertyChangedEventArgs', Sys.EventArgs);
 
Sys.INotifyDisposing = function Sys$INotifyDisposing() {
    /// <summary locid="M:J#Sys.INotifyDisposing.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
    function Sys$INotifyDisposing$add_disposing(handler) {
    /// <summary locid="E:J#Sys.INotifyDisposing.disposing" />
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$INotifyDisposing$remove_disposing(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
Sys.INotifyDisposing.prototype = {
    add_disposing: Sys$INotifyDisposing$add_disposing,
    remove_disposing: Sys$INotifyDisposing$remove_disposing
}
Sys.INotifyDisposing.registerInterface("Sys.INotifyDisposing");
 
Sys.Component = function Sys$Component() {
    /// <summary locid="M:J#Sys.Component.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    if (Sys.Application) Sys.Application.registerDisposableObject(this);
}
    function Sys$Component$get_events() {
        /// <value type="Sys.EventHandlerList" locid="P:J#Sys.Component.events"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Component$get_id() {
        /// <value type="String" locid="P:J#Sys.Component.id"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._id;
    }
    function Sys$Component$set_id(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        if (this._idSet) throw Error.invalidOperation(Sys.Res.componentCantSetIdTwice);
        this._idSet = true;
        var oldId = this.get_id();
        if (oldId && Sys.Application.findComponent(oldId)) throw Error.invalidOperation(Sys.Res.componentCantSetIdAfterAddedToApp);
        this._id = value;
    }
    function Sys$Component$get_isInitialized() {
        /// <value type="Boolean" locid="P:J#Sys.Component.isInitialized"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._initialized;
    }
    function Sys$Component$get_isUpdating() {
        /// <value type="Boolean" locid="P:J#Sys.Component.isUpdating"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._updating;
    }
    function Sys$Component$add_disposing(handler) {
        /// <summary locid="E:J#Sys.Component.disposing" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("disposing", handler);
    }
    function Sys$Component$remove_disposing(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("disposing", handler);
    }
    function Sys$Component$add_propertyChanged(handler) {
        /// <summary locid="E:J#Sys.Component.propertyChanged" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("propertyChanged", handler);
    }
    function Sys$Component$remove_propertyChanged(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("propertyChanged", handler);
    }
    function Sys$Component$beginUpdate() {
        this._updating = true;
    }
    function Sys$Component$dispose() {
        if (this._events) {
            var handler = this._events.getHandler("disposing");
            if (handler) {
                handler(this, Sys.EventArgs.Empty);
            }
        }
        delete this._events;
        Sys.Application.unregisterDisposableObject(this);
        Sys.Application.removeComponent(this);
    }
    function Sys$Component$endUpdate() {
        this._updating = false;
        if (!this._initialized) this.initialize();
        this.updated();
    }
    function Sys$Component$initialize() {
        this._initialized = true;
    }
    function Sys$Component$raisePropertyChanged(propertyName) {
        /// <summary locid="M:J#Sys.Component.raisePropertyChanged" />
        /// <param name="propertyName" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "propertyName", type: String}
        ]);
        if (e) throw e;
        if (!this._events) return;
        var handler = this._events.getHandler("propertyChanged");
        if (handler) {
            handler(this, new Sys.PropertyChangedEventArgs(propertyName));
        }
    }
    function Sys$Component$updated() {
    }
Sys.Component.prototype = {
    _id: null,
    _idSet: false,
    _initialized: false,
    _updating: false,
    get_events: Sys$Component$get_events,
    get_id: Sys$Component$get_id,
    set_id: Sys$Component$set_id,
    get_isInitialized: Sys$Component$get_isInitialized,
    get_isUpdating: Sys$Component$get_isUpdating,
    add_disposing: Sys$Component$add_disposing,
    remove_disposing: Sys$Component$remove_disposing,
    add_propertyChanged: Sys$Component$add_propertyChanged,
    remove_propertyChanged: Sys$Component$remove_propertyChanged,
    beginUpdate: Sys$Component$beginUpdate,
    dispose: Sys$Component$dispose,
    endUpdate: Sys$Component$endUpdate,
    initialize: Sys$Component$initialize,
    raisePropertyChanged: Sys$Component$raisePropertyChanged,
    updated: Sys$Component$updated
}
Sys.Component.registerClass('Sys.Component', null, Sys.IDisposable, Sys.INotifyPropertyChange, Sys.INotifyDisposing);
function Sys$Component$_setProperties(target, properties) {
    /// <summary locid="M:J#Sys.Component._setProperties" />
    /// <param name="target"></param>
    /// <param name="properties"></param>
    var e = Function._validateParams(arguments, [
        {name: "target"},
        {name: "properties"}
    ]);
    if (e) throw e;
    var current;
    var targetType = Object.getType(target);
    var isObject = (targetType === Object) || (targetType === Sys.UI.DomElement);
    var isComponent = Sys.Component.isInstanceOfType(target) && !target.get_isUpdating();
    if (isComponent) target.beginUpdate();
    for (var name in properties) {
        var val = properties[name];
        var getter = isObject ? null : target["get_" + name];
        if (isObject || typeof(getter) !== 'function') {
            var targetVal = target[name];
            if (!isObject && typeof(targetVal) === 'undefined') throw Error.invalidOperation(String.format(Sys.Res.propertyUndefined, name));
            if (!val || (typeof(val) !== 'object') || (isObject && !targetVal)) {
                target[name] = val;
            }
            else {
                Sys$Component$_setProperties(targetVal, val);
            }
        }
        else {
            var setter = target["set_" + name];
            if (typeof(setter) === 'function') {
                setter.apply(target, [val]);
            }
            else if (val instanceof Array) {
                current = getter.apply(target);
                if (!(current instanceof Array)) throw new Error.invalidOperation(String.format(Sys.Res.propertyNotAnArray, name));
                for (var i = 0, j = current.length, l= val.length; i < l; i++, j++) {
                    current[j] = val[i];
                }
            }
            else if ((typeof(val) === 'object') && (Object.getType(val) === Object)) {
                current = getter.apply(target);
                if ((typeof(current) === 'undefined') || (current === null)) throw new Error.invalidOperation(String.format(Sys.Res.propertyNullOrUndefined, name));
                Sys$Component$_setProperties(current, val);
            }
            else {
                throw new Error.invalidOperation(String.format(Sys.Res.propertyNotWritable, name));
            }
        }
    }
    if (isComponent) target.endUpdate();
}
function Sys$Component$_setReferences(component, references) {
    for (var name in references) {
        var setter = component["set_" + name];
        var reference = $find(references[name]);
        if (typeof(setter) !== 'function') throw new Error.invalidOperation(String.format(Sys.Res.propertyNotWritable, name));
        if (!reference) throw Error.invalidOperation(String.format(Sys.Res.referenceNotFound, references[name]));
        setter.apply(component, [reference]);
    }
}
var $create = Sys.Component.create = function Sys$Component$create(type, properties, events, references, element) {
    /// <summary locid="M:J#Sys.Component.create" />
    /// <param name="type" type="Type"></param>
    /// <param name="properties" optional="true" mayBeNull="true"></param>
    /// <param name="events" optional="true" mayBeNull="true"></param>
    /// <param name="references" optional="true" mayBeNull="true"></param>
    /// <param name="element" domElement="true" optional="true" mayBeNull="true"></param>
    /// <returns type="Sys.UI.Component"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "properties", mayBeNull: true, optional: true},
        {name: "events", mayBeNull: true, optional: true},
        {name: "references", mayBeNull: true, optional: true},
        {name: "element", mayBeNull: true, domElement: true, optional: true}
    ]);
    if (e) throw e;
    if (!type.inheritsFrom(Sys.Component)) {
        throw Error.argument('type', String.format(Sys.Res.createNotComponent, type.getName()));
    }
    if (type.inheritsFrom(Sys.UI.Behavior) || type.inheritsFrom(Sys.UI.Control)) {
        if (!element) throw Error.argument('element', Sys.Res.createNoDom);
    }
    else if (element) throw Error.argument('element', Sys.Res.createComponentOnDom);
    var component = (element ? new type(element): new type());
    var app = Sys.Application;
    var creatingComponents = app.get_isCreatingComponents();
    component.beginUpdate();
    if (properties) {
        Sys$Component$_setProperties(component, properties);
    }
    if (events) {
        for (var name in events) {
            if (!(component["add_" + name] instanceof Function)) throw new Error.invalidOperation(String.format(Sys.Res.undefinedEvent, name));
            if (!(events[name] instanceof Function)) throw new Error.invalidOperation(Sys.Res.eventHandlerNotFunction);
            component["add_" + name](events[name]);
        }
    }
    if (component.get_id()) {
        app.addComponent(component);
    }
    if (creatingComponents) {
        app._createdComponents[app._createdComponents.length] = component;
        if (references) {
            app._addComponentToSecondPass(component, references);
        }
        else {
            component.endUpdate();
        }
    }
    else {
        if (references) {
            Sys$Component$_setReferences(component, references);
        }
        component.endUpdate();
    }
    return component;
}
 
Sys.UI.MouseButton = function Sys$UI$MouseButton() {
    /// <summary locid="M:J#Sys.UI.MouseButton.#ctor" />
    /// <field name="leftButton" type="Number" integer="true" static="true" locid="F:J#Sys.UI.MouseButton.leftButton"></field>
    /// <field name="middleButton" type="Number" integer="true" static="true" locid="F:J#Sys.UI.MouseButton.middleButton"></field>
    /// <field name="rightButton" type="Number" integer="true" static="true" locid="F:J#Sys.UI.MouseButton.rightButton"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
Sys.UI.MouseButton.prototype = {
    leftButton: 0,
    middleButton: 1,
    rightButton: 2
}
Sys.UI.MouseButton.registerEnum("Sys.UI.MouseButton");
 
Sys.UI.Key = function Sys$UI$Key() {
    /// <summary locid="M:J#Sys.UI.Key.#ctor" />
    /// <field name="backspace" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.backspace"></field>
    /// <field name="tab" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.tab"></field>
    /// <field name="enter" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.enter"></field>
    /// <field name="esc" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.esc"></field>
    /// <field name="space" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.space"></field>
    /// <field name="pageUp" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.pageUp"></field>
    /// <field name="pageDown" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.pageDown"></field>
    /// <field name="end" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.end"></field>
    /// <field name="home" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.home"></field>
    /// <field name="left" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.left"></field>
    /// <field name="up" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.up"></field>
    /// <field name="right" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.right"></field>
    /// <field name="down" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.down"></field>
    /// <field name="del" type="Number" integer="true" static="true" locid="F:J#Sys.UI.Key.del"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
Sys.UI.Key.prototype = {
    backspace: 8,
    tab: 9,
    enter: 13,
    esc: 27,
    space: 32,
    pageUp: 33,
    pageDown: 34,
    end: 35,
    home: 36,
    left: 37,
    up: 38,
    right: 39,
    down: 40,
    del: 127
}
Sys.UI.Key.registerEnum("Sys.UI.Key");
 
Sys.UI.Point = function Sys$UI$Point(x, y) {
    /// <summary locid="M:J#Sys.UI.Point.#ctor" />
    /// <param name="x" type="Number" integer="true"></param>
    /// <param name="y" type="Number" integer="true"></param>
    /// <field name="x" type="Number" integer="true" locid="F:J#Sys.UI.Point.x"></field>
    /// <field name="y" type="Number" integer="true" locid="F:J#Sys.UI.Point.y"></field>
    var e = Function._validateParams(arguments, [
        {name: "x", type: Number, integer: true},
        {name: "y", type: Number, integer: true}
    ]);
    if (e) throw e;
    this.x = x;
    this.y = y;
}
Sys.UI.Point.registerClass('Sys.UI.Point');
 
Sys.UI.Bounds = function Sys$UI$Bounds(x, y, width, height) {
    /// <summary locid="M:J#Sys.UI.Bounds.#ctor" />
    /// <param name="x" type="Number" integer="true"></param>
    /// <param name="y" type="Number" integer="true"></param>
    /// <param name="width" type="Number" integer="true"></param>
    /// <param name="height" type="Number" integer="true"></param>
    /// <field name="x" type="Number" integer="true" locid="F:J#Sys.UI.Bounds.x"></field>
    /// <field name="y" type="Number" integer="true" locid="F:J#Sys.UI.Bounds.y"></field>
    /// <field name="width" type="Number" integer="true" locid="F:J#Sys.UI.Bounds.width"></field>
    /// <field name="height" type="Number" integer="true" locid="F:J#Sys.UI.Bounds.height"></field>
    var e = Function._validateParams(arguments, [
        {name: "x", type: Number, integer: true},
        {name: "y", type: Number, integer: true},
        {name: "width", type: Number, integer: true},
        {name: "height", type: Number, integer: true}
    ]);
    if (e) throw e;
    this.x = x;
    this.y = y;
    this.height = height;
    this.width = width;
}
Sys.UI.Bounds.registerClass('Sys.UI.Bounds');
 
Sys.UI.DomEvent = function Sys$UI$DomEvent(eventObject) {
    /// <summary locid="M:J#Sys.UI.DomEvent.#ctor" />
    /// <param name="eventObject"></param>
    /// <field name="altKey" type="Boolean" locid="F:J#Sys.UI.DomEvent.altKey"></field>
    /// <field name="button" type="Sys.UI.MouseButton" locid="F:J#Sys.UI.DomEvent.button"></field>
    /// <field name="charCode" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.charCode"></field>
    /// <field name="clientX" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.clientX"></field>
    /// <field name="clientY" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.clientY"></field>
    /// <field name="ctrlKey" type="Boolean" locid="F:J#Sys.UI.DomEvent.ctrlKey"></field>
    /// <field name="keyCode" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.keyCode"></field>
    /// <field name="offsetX" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.offsetX"></field>
    /// <field name="offsetY" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.offsetY"></field>
    /// <field name="screenX" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.screenX"></field>
    /// <field name="screenY" type="Number" integer="true" locid="F:J#Sys.UI.DomEvent.screenY"></field>
    /// <field name="shiftKey" type="Boolean" locid="F:J#Sys.UI.DomEvent.shiftKey"></field>
    /// <field name="target" locid="F:J#Sys.UI.DomEvent.target"></field>
    /// <field name="type" type="String" locid="F:J#Sys.UI.DomEvent.type"></field>
    var e = Function._validateParams(arguments, [
        {name: "eventObject"}
    ]);
    if (e) throw e;
    var ev = eventObject;
    var etype = this.type = ev.type.toLowerCase();
    this.rawEvent = ev;
    this.altKey = ev.altKey;
    if (typeof(ev.button) !== 'undefined') {
        this.button = (typeof(ev.which) !== 'undefined') ? ev.button :
            (ev.button === 4) ? Sys.UI.MouseButton.middleButton :
            (ev.button === 2) ? Sys.UI.MouseButton.rightButton :
            Sys.UI.MouseButton.leftButton;
    }
    if (etype === 'keypress') {
        this.charCode = ev.charCode || ev.keyCode;
    }
    else if (ev.keyCode && (ev.keyCode === 46)) {
        this.keyCode = 127;
    }
    else {
        this.keyCode = ev.keyCode;
    }
    this.clientX = ev.clientX;
    this.clientY = ev.clientY;
    this.ctrlKey = ev.ctrlKey;
    this.target = ev.target ? ev.target : ev.srcElement;
    if (!etype.startsWith('key')) {
        if ((typeof(ev.offsetX) !== 'undefined') && (typeof(ev.offsetY) !== 'undefined')) {
            this.offsetX = ev.offsetX;
            this.offsetY = ev.offsetY;
        }
        else if (this.target && (this.target.nodeType !== 3) && (typeof(ev.clientX) === 'number')) {
            var loc = Sys.UI.DomElement.getLocation(this.target);
            var w = Sys.UI.DomElement._getWindow(this.target);
            this.offsetX = (w.pageXOffset || 0) + ev.clientX - loc.x;
            this.offsetY = (w.pageYOffset || 0) + ev.clientY - loc.y;
        }
    }
    this.screenX = ev.screenX;
    this.screenY = ev.screenY;
    this.shiftKey = ev.shiftKey;
}
    function Sys$UI$DomEvent$preventDefault() {
        /// <summary locid="M:J#Sys.UI.DomEvent.preventDefault" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this.rawEvent.preventDefault) {
            this.rawEvent.preventDefault();
        }
        else if (window.event) {
            this.rawEvent.returnValue = false;
        }
    }
    function Sys$UI$DomEvent$stopPropagation() {
        /// <summary locid="M:J#Sys.UI.DomEvent.stopPropagation" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this.rawEvent.stopPropagation) {
            this.rawEvent.stopPropagation();
        }
        else if (window.event) {
            this.rawEvent.cancelBubble = true;
        }
    }
Sys.UI.DomEvent.prototype = {
    preventDefault: Sys$UI$DomEvent$preventDefault,
    stopPropagation: Sys$UI$DomEvent$stopPropagation
}
Sys.UI.DomEvent.registerClass('Sys.UI.DomEvent');
var $addHandler = Sys.UI.DomEvent.addHandler = function Sys$UI$DomEvent$addHandler(element, eventName, handler, autoRemove) {
    /// <summary locid="M:J#Sys.UI.DomEvent.addHandler" />
    /// <param name="element"></param>
    /// <param name="eventName" type="String"></param>
    /// <param name="handler" type="Function"></param>
    /// <param name="autoRemove" type="Boolean" optional="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element"},
        {name: "eventName", type: String},
        {name: "handler", type: Function},
        {name: "autoRemove", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    Sys.UI.DomEvent._ensureDomNode(element);
    if (eventName === "error") throw Error.invalidOperation(Sys.Res.addHandlerCantBeUsedForError);
    if (!element._events) {
        element._events = {};
    }
    var eventCache = element._events[eventName];
    if (!eventCache) {
        element._events[eventName] = eventCache = [];
    }
    var browserHandler;
    if (element.addEventListener) {
        browserHandler = function(e) {
            return handler.call(element, new Sys.UI.DomEvent(e));
        }
        element.addEventListener(eventName, browserHandler, false);
    }
    else if (element.attachEvent) {
        browserHandler = function() {
            var e = {};
            try {e = Sys.UI.DomElement._getWindow(element).event} catch(ex) {}
            return handler.call(element, new Sys.UI.DomEvent(e));
        }
        element.attachEvent('on' + eventName, browserHandler);
    }
    eventCache[eventCache.length] = {handler: handler, browserHandler: browserHandler, autoRemove: autoRemove };
    if (autoRemove) {
        var d = element.dispose;
        if (d !== Sys.UI.DomEvent._disposeHandlers) {
            element.dispose = Sys.UI.DomEvent._disposeHandlers;
            if (typeof(d) !== "undefined") {
                element._chainDispose = d;
            }
        }
    }
}
var $addHandlers = Sys.UI.DomEvent.addHandlers = function Sys$UI$DomEvent$addHandlers(element, events, handlerOwner, autoRemove) {
    /// <summary locid="M:J#Sys.UI.DomEvent.addHandlers" />
    /// <param name="element"></param>
    /// <param name="events" type="Object"></param>
    /// <param name="handlerOwner" optional="true"></param>
    /// <param name="autoRemove" type="Boolean" optional="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element"},
        {name: "events", type: Object},
        {name: "handlerOwner", optional: true},
        {name: "autoRemove", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    Sys.UI.DomEvent._ensureDomNode(element);
    for (var name in events) {
        var handler = events[name];
        if (typeof(handler) !== 'function') throw Error.invalidOperation(Sys.Res.cantAddNonFunctionhandler);
        if (handlerOwner) {
            handler = Function.createDelegate(handlerOwner, handler);
        }
        $addHandler(element, name, handler, autoRemove || false);
    }
}
var $clearHandlers = Sys.UI.DomEvent.clearHandlers = function Sys$UI$DomEvent$clearHandlers(element) {
    /// <summary locid="M:J#Sys.UI.DomEvent.clearHandlers" />
    /// <param name="element"></param>
    var e = Function._validateParams(arguments, [
        {name: "element"}
    ]);
    if (e) throw e;
    Sys.UI.DomEvent._ensureDomNode(element);
    Sys.UI.DomEvent._clearHandlers(element, false);
}
Sys.UI.DomEvent._clearHandlers = function Sys$UI$DomEvent$_clearHandlers(element, autoRemoving) {
    if (element._events) {
        var cache = element._events;
        for (var name in cache) {
            var handlers = cache[name];
            for (var i = handlers.length - 1; i >= 0; i--) {
                var entry = handlers[i];
                if (!autoRemoving || entry.autoRemove) {
                    $removeHandler(element, name, entry.handler);
                }
            }
        }
        element._events = null;
    }
}
Sys.UI.DomEvent._disposeHandlers = function Sys$UI$DomEvent$_disposeHandlers() {
    Sys.UI.DomEvent._clearHandlers(this, true);
    var d = this._chainDispose, type = typeof(d);
    if (type !== "undefined") {
        this.dispose = d;
        this._chainDispose = null;
        if (type === "function") {
            this.dispose();
        }
    }
}
var $removeHandler = Sys.UI.DomEvent.removeHandler = function Sys$UI$DomEvent$removeHandler(element, eventName, handler) {
    /// <summary locid="M:J#Sys.UI.DomEvent.removeHandler" />
    /// <param name="element"></param>
    /// <param name="eventName" type="String"></param>
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "element"},
        {name: "eventName", type: String},
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    Sys.UI.DomEvent._removeHandler(element, eventName, handler);
}
Sys.UI.DomEvent._removeHandler = function Sys$UI$DomEvent$_removeHandler(element, eventName, handler) {
    Sys.UI.DomEvent._ensureDomNode(element);
    var browserHandler = null;
    if ((typeof(element._events) !== 'object') || !element._events) throw Error.invalidOperation(Sys.Res.eventHandlerInvalid);
    var cache = element._events[eventName];
    if (!(cache instanceof Array)) throw Error.invalidOperation(Sys.Res.eventHandlerInvalid);
    for (var i = 0, l = cache.length; i < l; i++) {
        if (cache[i].handler === handler) {
            browserHandler = cache[i].browserHandler;
            break;
        }
    }
    if (typeof(browserHandler) !== 'function') throw Error.invalidOperation(Sys.Res.eventHandlerInvalid);
    if (element.removeEventListener) {
        element.removeEventListener(eventName, browserHandler, false);
    }
    else if (element.detachEvent) {
        element.detachEvent('on' + eventName, browserHandler);
    }
    cache.splice(i, 1);
}
Sys.UI.DomEvent._ensureDomNode = function Sys$UI$DomEvent$_ensureDomNode(element) {
    if (element.tagName && (element.tagName.toUpperCase() === "SCRIPT")) return;
    
    var doc = element.ownerDocument || element.document || element;
    if ((typeof(element.document) !== 'object') && (element != doc) && (typeof(element.nodeType) !== 'number')) {
        throw Error.argument("element", Sys.Res.argumentDomNode);
    }
}
 
Sys.UI.DomElement = function Sys$UI$DomElement() {
    /// <summary locid="M:J#Sys.UI.DomElement.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
Sys.UI.DomElement.registerClass('Sys.UI.DomElement');
Sys.UI.DomElement.addCssClass = function Sys$UI$DomElement$addCssClass(element, className) {
    /// <summary locid="M:J#Sys.UI.DomElement.addCssClass" />
    /// <param name="element" domElement="true"></param>
    /// <param name="className" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "className", type: String}
    ]);
    if (e) throw e;
    if (!Sys.UI.DomElement.containsCssClass(element, className)) {
        if (element.className === '') {
            element.className = className;
        }
        else {
            element.className += ' ' + className;
        }
    }
}
Sys.UI.DomElement.containsCssClass = function Sys$UI$DomElement$containsCssClass(element, className) {
    /// <summary locid="M:J#Sys.UI.DomElement.containsCssClass" />
    /// <param name="element" domElement="true"></param>
    /// <param name="className" type="String"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "className", type: String}
    ]);
    if (e) throw e;
    return Array.contains(element.className.split(' '), className);
}
Sys.UI.DomElement.getBounds = function Sys$UI$DomElement$getBounds(element) {
    /// <summary locid="M:J#Sys.UI.DomElement.getBounds" />
    /// <param name="element" domElement="true"></param>
    /// <returns type="Sys.UI.Bounds"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    var offset = Sys.UI.DomElement.getLocation(element);
    return new Sys.UI.Bounds(offset.x, offset.y, element.offsetWidth || 0, element.offsetHeight || 0);
}
var $get = Sys.UI.DomElement.getElementById = function Sys$UI$DomElement$getElementById(id, element) {
    /// <summary locid="M:J#Sys.UI.DomElement.getElementById" />
    /// <param name="id" type="String"></param>
    /// <param name="element" domElement="true" optional="true" mayBeNull="true"></param>
    /// <returns domElement="true" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "id", type: String},
        {name: "element", mayBeNull: true, domElement: true, optional: true}
    ]);
    if (e) throw e;
    if (!element) return document.getElementById(id);
    if (element.getElementById) return element.getElementById(id);
    var nodeQueue = [];
    var childNodes = element.childNodes;
    for (var i = 0; i < childNodes.length; i++) {
        var node = childNodes[i];
        if (node.nodeType == 1) {
            nodeQueue[nodeQueue.length] = node;
        }
    }
    while (nodeQueue.length) {
        node = nodeQueue.shift();
        if (node.id == id) {
            return node;
        }
        childNodes = node.childNodes;
        for (i = 0; i < childNodes.length; i++) {
            node = childNodes[i];
            if (node.nodeType == 1) {
                nodeQueue[nodeQueue.length] = node;
            }
        }
    }
    return null;
}
if (document.documentElement.getBoundingClientRect) {
    Sys.UI.DomElement.getLocation = function Sys$UI$DomElement$getLocation(element) {
        /// <summary locid="M:J#Sys.UI.DomElement.getLocation" />
        /// <param name="element" domElement="true"></param>
        /// <returns type="Sys.UI.Point"></returns>
        var e = Function._validateParams(arguments, [
            {name: "element", domElement: true}
        ]);
        if (e) throw e;
        if (element.self || element.nodeType === 9) return new Sys.UI.Point(0,0);
        var clientRect = element.getBoundingClientRect();
        if (!clientRect) {
            return new Sys.UI.Point(0,0);
        }
        var documentElement = element.ownerDocument.documentElement,
            offsetX = Math.floor(clientRect.left + 0.5) + documentElement.scrollLeft,
            offsetY = Math.floor(clientRect.top + 0.5) + documentElement.scrollTop;
        if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
            try {
                var f = element.ownerDocument.parentWindow.frameElement || null;
                if (f) {
                    var offset = (f.frameBorder === "0" || f.frameBorder === "no") ? 2 : 0;
                    offsetX += offset;
                    offsetY += offset;
                }
            }
            catch(ex) {
            }
            if (Sys.Browser.version <= 7) {
                
                var multiplier, before, rect, d = document.createElement("div");
                d.style.cssText = "position:absolute !important;left:0px !important;right:0px !important;height:0px !important;width:1px !important;display:hidden !important";
                try {
                    before = document.body.childNodes[0];
                    document.body.insertBefore(d, before);
                    rect = d.getBoundingClientRect();
                    document.body.removeChild(d);
                    multiplier = (rect.right - rect.left);
                }
                catch (e) {
                }
                if (multiplier && (multiplier !== 1)) {
                    offsetX = Math.floor(offsetX / multiplier);
                    offsetY = Math.floor(offsetY / multiplier);
                }
            }        
            if ((document.documentMode || 0) < 8) {
                offsetX -= 2;
                offsetY -= 2;
            }
        }
        return new Sys.UI.Point(offsetX, offsetY);
    }
}
else if (Sys.Browser.agent === Sys.Browser.Safari) {
    Sys.UI.DomElement.getLocation = function Sys$UI$DomElement$getLocation(element) {
        /// <summary locid="M:J#Sys.UI.DomElement.getLocation" />
        /// <param name="element" domElement="true"></param>
        /// <returns type="Sys.UI.Point"></returns>
        var e = Function._validateParams(arguments, [
            {name: "element", domElement: true}
        ]);
        if (e) throw e;
        if ((element.window && (element.window === element)) || element.nodeType === 9) return new Sys.UI.Point(0,0);
        var offsetX = 0, offsetY = 0,
            parent,
            previous = null,
            previousStyle = null,
            currentStyle;
        for (parent = element; parent; previous = parent, previousStyle = currentStyle, parent = parent.offsetParent) {
            currentStyle = Sys.UI.DomElement._getCurrentStyle(parent);
            var tagName = parent.tagName ? parent.tagName.toUpperCase() : null;
            if ((parent.offsetLeft || parent.offsetTop) &&
                ((tagName !== "BODY") || (!previousStyle || previousStyle.position !== "absolute"))) {
                offsetX += parent.offsetLeft;
                offsetY += parent.offsetTop;
            }
            if (previous && Sys.Browser.version >= 3) {
                offsetX += parseInt(currentStyle.borderLeftWidth);
                offsetY += parseInt(currentStyle.borderTopWidth);
            }
        }
        currentStyle = Sys.UI.DomElement._getCurrentStyle(element);
        var elementPosition = currentStyle ? currentStyle.position : null;
        if (!elementPosition || (elementPosition !== "absolute")) {
            for (parent = element.parentNode; parent; parent = parent.parentNode) {
                tagName = parent.tagName ? parent.tagName.toUpperCase() : null;
                if ((tagName !== "BODY") && (tagName !== "HTML") && (parent.scrollLeft || parent.scrollTop)) {
                    offsetX -= (parent.scrollLeft || 0);
                    offsetY -= (parent.scrollTop || 0);
                }
                currentStyle = Sys.UI.DomElement._getCurrentStyle(parent);
                var parentPosition = currentStyle ? currentStyle.position : null;
                if (parentPosition && (parentPosition === "absolute")) break;
            }
        }
        return new Sys.UI.Point(offsetX, offsetY);
    }
}
else {
    Sys.UI.DomElement.getLocation = function Sys$UI$DomElement$getLocation(element) {
        /// <summary locid="M:J#Sys.UI.DomElement.getLocation" />
        /// <param name="element" domElement="true"></param>
        /// <returns type="Sys.UI.Point"></returns>
        var e = Function._validateParams(arguments, [
            {name: "element", domElement: true}
        ]);
        if (e) throw e;
        if ((element.window && (element.window === element)) || element.nodeType === 9) return new Sys.UI.Point(0,0);
        var offsetX = 0, offsetY = 0,
            parent,
            previous = null,
            previousStyle = null,
            currentStyle = null;
        for (parent = element; parent; previous = parent, previousStyle = currentStyle, parent = parent.offsetParent) {
            var tagName = parent.tagName ? parent.tagName.toUpperCase() : null;
            currentStyle = Sys.UI.DomElement._getCurrentStyle(parent);
            if ((parent.offsetLeft || parent.offsetTop) &&
                !((tagName === "BODY") &&
                (!previousStyle || previousStyle.position !== "absolute"))) {
                offsetX += parent.offsetLeft;
                offsetY += parent.offsetTop;
            }
            if (previous !== null && currentStyle) {
                if ((tagName !== "TABLE") && (tagName !== "TD") && (tagName !== "HTML")) {
                    offsetX += parseInt(currentStyle.borderLeftWidth) || 0;
                    offsetY += parseInt(currentStyle.borderTopWidth) || 0;
                }
                if (tagName === "TABLE" &&
                    (currentStyle.position === "relative" || currentStyle.position === "absolute")) {
                    offsetX += parseInt(currentStyle.marginLeft) || 0;
                    offsetY += parseInt(currentStyle.marginTop) || 0;
                }
            }
        }
        currentStyle = Sys.UI.DomElement._getCurrentStyle(element);
        var elementPosition = currentStyle ? currentStyle.position : null;
        if (!elementPosition || (elementPosition !== "absolute")) {
            for (parent = element.parentNode; parent; parent = parent.parentNode) {
                tagName = parent.tagName ? parent.tagName.toUpperCase() : null;
                if ((tagName !== "BODY") && (tagName !== "HTML") && (parent.scrollLeft || parent.scrollTop)) {
                    offsetX -= (parent.scrollLeft || 0);
                    offsetY -= (parent.scrollTop || 0);
                    currentStyle = Sys.UI.DomElement._getCurrentStyle(parent);
                    if (currentStyle) {
                        offsetX += parseInt(currentStyle.borderLeftWidth) || 0;
                        offsetY += parseInt(currentStyle.borderTopWidth) || 0;
                    }
                }
            }
        }
        return new Sys.UI.Point(offsetX, offsetY);
    }
}
Sys.UI.DomElement.isDomElement = function Sys$UI$DomElement$isDomElement(obj) {
    /// <summary locid="M:J#Sys.UI.DomElement.isDomElement" />
    /// <param name="obj"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "obj"}
    ]);
    if (e) throw e;
    return Sys._isDomElement(obj);
}
Sys.UI.DomElement.removeCssClass = function Sys$UI$DomElement$removeCssClass(element, className) {
    /// <summary locid="M:J#Sys.UI.DomElement.removeCssClass" />
    /// <param name="element" domElement="true"></param>
    /// <param name="className" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "className", type: String}
    ]);
    if (e) throw e;
    var currentClassName = ' ' + element.className + ' ';
    var index = currentClassName.indexOf(' ' + className + ' ');
    if (index >= 0) {
        element.className = (currentClassName.substr(0, index) + ' ' +
            currentClassName.substring(index + className.length + 1, currentClassName.length)).trim();
    }
}
Sys.UI.DomElement.resolveElement = function Sys$UI$DomElement$resolveElement(elementOrElementId, containerElement) {
    /// <summary locid="M:J#Sys.UI.DomElement.resolveElement" />
    /// <param name="elementOrElementId" mayBeNull="true"></param>
    /// <param name="containerElement" domElement="true" optional="true" mayBeNull="true"></param>
    /// <returns domElement="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "elementOrElementId", mayBeNull: true},
        {name: "containerElement", mayBeNull: true, domElement: true, optional: true}
    ]);
    if (e) throw e;
    var el = elementOrElementId;
    if (!el) return null;
    if (typeof(el) === "string") {
        el = Sys.UI.DomElement.getElementById(el, containerElement);
        if (!el) {
            throw Error.argument("elementOrElementId", String.format(Sys.Res.elementNotFound, elementOrElementId));
        }
    }
    else if(!Sys.UI.DomElement.isDomElement(el)) {
        throw Error.argument("elementOrElementId", Sys.Res.expectedElementOrId);
    }
    return el;
}
Sys.UI.DomElement.raiseBubbleEvent = function Sys$UI$DomElement$raiseBubbleEvent(source, args) {
    /// <summary locid="M:J#Sys.UI.DomElement.raiseBubbleEvent" />
    /// <param name="source" domElement="true"></param>
    /// <param name="args" type="Sys.EventArgs"></param>
    var e = Function._validateParams(arguments, [
        {name: "source", domElement: true},
        {name: "args", type: Sys.EventArgs}
    ]);
    if (e) throw e;
    var target = source;
    while (target) {
        var control = target.control;
        if (control && control.onBubbleEvent && control.raiseBubbleEvent) {
            Sys.UI.DomElement._raiseBubbleEventFromControl(control, source, args);
            return;
        }
        target = target.parentNode;
    }
}
Sys.UI.DomElement._raiseBubbleEventFromControl = function Sys$UI$DomElement$_raiseBubbleEventFromControl(control, source, args) {
    if (!control.onBubbleEvent(source, args)) {
        control._raiseBubbleEvent(source, args);
    }
}
Sys.UI.DomElement.setLocation = function Sys$UI$DomElement$setLocation(element, x, y) {
    /// <summary locid="M:J#Sys.UI.DomElement.setLocation" />
    /// <param name="element" domElement="true"></param>
    /// <param name="x" type="Number" integer="true"></param>
    /// <param name="y" type="Number" integer="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "x", type: Number, integer: true},
        {name: "y", type: Number, integer: true}
    ]);
    if (e) throw e;
    var style = element.style;
    style.position = 'absolute';
    style.left = x + "px";
    style.top = y + "px";
}
Sys.UI.DomElement.toggleCssClass = function Sys$UI$DomElement$toggleCssClass(element, className) {
    /// <summary locid="M:J#Sys.UI.DomElement.toggleCssClass" />
    /// <param name="element" domElement="true"></param>
    /// <param name="className" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "className", type: String}
    ]);
    if (e) throw e;
    if (Sys.UI.DomElement.containsCssClass(element, className)) {
        Sys.UI.DomElement.removeCssClass(element, className);
    }
    else {
        Sys.UI.DomElement.addCssClass(element, className);
    }
}
Sys.UI.DomElement.getVisibilityMode = function Sys$UI$DomElement$getVisibilityMode(element) {
    /// <summary locid="M:J#Sys.UI.DomElement.getVisibilityMode" />
    /// <param name="element" domElement="true"></param>
    /// <returns type="Sys.UI.VisibilityMode"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    return (element._visibilityMode === Sys.UI.VisibilityMode.hide) ?
        Sys.UI.VisibilityMode.hide :
        Sys.UI.VisibilityMode.collapse;
}
Sys.UI.DomElement.setVisibilityMode = function Sys$UI$DomElement$setVisibilityMode(element, value) {
    /// <summary locid="M:J#Sys.UI.DomElement.setVisibilityMode" />
    /// <param name="element" domElement="true"></param>
    /// <param name="value" type="Sys.UI.VisibilityMode"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "value", type: Sys.UI.VisibilityMode}
    ]);
    if (e) throw e;
    Sys.UI.DomElement._ensureOldDisplayMode(element);
    if (element._visibilityMode !== value) {
        element._visibilityMode = value;
        if (Sys.UI.DomElement.getVisible(element) === false) {
            if (element._visibilityMode === Sys.UI.VisibilityMode.hide) {
                element.style.display = element._oldDisplayMode;
            }
            else {
                element.style.display = 'none';
            }
        }
        element._visibilityMode = value;
    }
}
Sys.UI.DomElement.getVisible = function Sys$UI$DomElement$getVisible(element) {
    /// <summary locid="M:J#Sys.UI.DomElement.getVisible" />
    /// <param name="element" domElement="true"></param>
    /// <returns type="Boolean"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    var style = element.currentStyle || Sys.UI.DomElement._getCurrentStyle(element);
    if (!style) return true;
    return (style.visibility !== 'hidden') && (style.display !== 'none');
}
Sys.UI.DomElement.setVisible = function Sys$UI$DomElement$setVisible(element, value) {
    /// <summary locid="M:J#Sys.UI.DomElement.setVisible" />
    /// <param name="element" domElement="true"></param>
    /// <param name="value" type="Boolean"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "value", type: Boolean}
    ]);
    if (e) throw e;
    if (value !== Sys.UI.DomElement.getVisible(element)) {
        Sys.UI.DomElement._ensureOldDisplayMode(element);
        element.style.visibility = value ? 'visible' : 'hidden';
        if (value || (element._visibilityMode === Sys.UI.VisibilityMode.hide)) {
            element.style.display = element._oldDisplayMode;
        }
        else {
            element.style.display = 'none';
        }
    }
}
Sys.UI.DomElement._ensureOldDisplayMode = function Sys$UI$DomElement$_ensureOldDisplayMode(element) {
    if (!element._oldDisplayMode) {
        var style = element.currentStyle || Sys.UI.DomElement._getCurrentStyle(element);
        element._oldDisplayMode = style ? style.display : null;
        if (!element._oldDisplayMode || element._oldDisplayMode === 'none') {
            switch(element.tagName.toUpperCase()) {
                case 'DIV': case 'P': case 'ADDRESS': case 'BLOCKQUOTE': case 'BODY': case 'COL':
                case 'COLGROUP': case 'DD': case 'DL': case 'DT': case 'FIELDSET': case 'FORM':
                case 'H1': case 'H2': case 'H3': case 'H4': case 'H5': case 'H6': case 'HR':
                case 'IFRAME': case 'LEGEND': case 'OL': case 'PRE': case 'TABLE': case 'TD':
                case 'TH': case 'TR': case 'UL':
                    element._oldDisplayMode = 'block';
                    break;
                case 'LI':
                    element._oldDisplayMode = 'list-item';
                    break;
                default:
                    element._oldDisplayMode = 'inline';
            }
        }
    }
}
Sys.UI.DomElement._getWindow = function Sys$UI$DomElement$_getWindow(element) {
    var doc = element.ownerDocument || element.document || element;
    return doc.defaultView || doc.parentWindow;
}
Sys.UI.DomElement._getCurrentStyle = function Sys$UI$DomElement$_getCurrentStyle(element) {
    if (element.nodeType === 3) return null;
    var w = Sys.UI.DomElement._getWindow(element);
    if (element.documentElement) element = element.documentElement;
    var computedStyle = (w && (element !== w) && w.getComputedStyle) ?
        w.getComputedStyle(element, null) :
        element.currentStyle || element.style;
    if (!computedStyle && (Sys.Browser.agent === Sys.Browser.Safari) && element.style) {
        var oldDisplay = element.style.display;
        var oldPosition = element.style.position;
        element.style.position = 'absolute';
        element.style.display = 'block';
        var style = w.getComputedStyle(element, null);
        element.style.display = oldDisplay;
        element.style.position = oldPosition;
        computedStyle = {};
        for (var n in style) {
            computedStyle[n] = style[n];
        }
        computedStyle.display = 'none';
    }
    return computedStyle;
}
 
Sys.IContainer = function Sys$IContainer() {
    throw Error.notImplemented();
}
    function Sys$IContainer$addComponent(component) {
        /// <summary locid="M:J#Sys.IContainer.addComponent" />
        /// <param name="component" type="Sys.Component"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Sys.Component}
        ]);
        if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$IContainer$removeComponent(component) {
        /// <summary locid="M:J#Sys.IContainer.removeComponent" />
        /// <param name="component" type="Sys.Component"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Sys.Component}
        ]);
        if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$IContainer$findComponent(id) {
        /// <summary locid="M:J#Sys.IContainer.findComponent" />
        /// <param name="id" type="String"></param>
        /// <returns type="Sys.Component"></returns>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String}
        ]);
        if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$IContainer$getComponents() {
        /// <summary locid="M:J#Sys.IContainer.getComponents" />
        /// <returns type="Array" elementType="Sys.Component"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
Sys.IContainer.prototype = {
    addComponent: Sys$IContainer$addComponent,
    removeComponent: Sys$IContainer$removeComponent,
    findComponent: Sys$IContainer$findComponent,
    getComponents: Sys$IContainer$getComponents
}
Sys.IContainer.registerInterface("Sys.IContainer");
 
Sys.ApplicationLoadEventArgs = function Sys$ApplicationLoadEventArgs(components, isPartialLoad) {
    /// <summary locid="M:J#Sys.ApplicationLoadEventArgs.#ctor" />
    /// <param name="components" type="Array" elementType="Sys.Component"></param>
    /// <param name="isPartialLoad" type="Boolean"></param>
    var e = Function._validateParams(arguments, [
        {name: "components", type: Array, elementType: Sys.Component},
        {name: "isPartialLoad", type: Boolean}
    ]);
    if (e) throw e;
    Sys.ApplicationLoadEventArgs.initializeBase(this);
    this._components = components;
    this._isPartialLoad = isPartialLoad;
}
 
    function Sys$ApplicationLoadEventArgs$get_components() {
        /// <value type="Array" elementType="Sys.Component" locid="P:J#Sys.ApplicationLoadEventArgs.components"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._components;
    }
    function Sys$ApplicationLoadEventArgs$get_isPartialLoad() {
        /// <value type="Boolean" locid="P:J#Sys.ApplicationLoadEventArgs.isPartialLoad"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._isPartialLoad;
    }
Sys.ApplicationLoadEventArgs.prototype = {
    get_components: Sys$ApplicationLoadEventArgs$get_components,
    get_isPartialLoad: Sys$ApplicationLoadEventArgs$get_isPartialLoad
}
Sys.ApplicationLoadEventArgs.registerClass('Sys.ApplicationLoadEventArgs', Sys.EventArgs);
 
Sys._Application = function Sys$_Application() {
    /// <summary locid="M:J#Sys.Application.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    Sys._Application.initializeBase(this);
    this._disposableObjects = [];
    this._components = {};
    this._createdComponents = [];
    this._secondPassComponents = [];
    this._unloadHandlerDelegate = Function.createDelegate(this, this._unloadHandler);
    Sys.UI.DomEvent.addHandler(window, "unload", this._unloadHandlerDelegate);
    this._domReady();
}
    function Sys$_Application$get_isCreatingComponents() {
        /// <value type="Boolean" locid="P:J#Sys.Application.isCreatingComponents"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._creatingComponents;
    }
    function Sys$_Application$get_isDisposing() {
        /// <value type="Boolean" locid="P:J#Sys.Application.isDisposing"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._disposing;
    }
    function Sys$_Application$add_init(handler) {
        /// <summary locid="E:J#Sys.Application.init" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        if (this._initialized) {
            handler(this, Sys.EventArgs.Empty);
        }
        else {
            this.get_events().addHandler("init", handler);
        }
    }
    function Sys$_Application$remove_init(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("init", handler);
    }
    function Sys$_Application$add_load(handler) {
        /// <summary locid="E:J#Sys.Application.load" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("load", handler);
    }
    function Sys$_Application$remove_load(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("load", handler);
    }
    function Sys$_Application$add_unload(handler) {
        /// <summary locid="E:J#Sys.Application.unload" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("unload", handler);
    }
    function Sys$_Application$remove_unload(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("unload", handler);
    }
    function Sys$_Application$addComponent(component) {
        /// <summary locid="M:J#Sys.Application.addComponent" />
        /// <param name="component" type="Sys.Component"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Sys.Component}
        ]);
        if (e) throw e;
        var id = component.get_id();
        if (!id) throw Error.invalidOperation(Sys.Res.cantAddWithoutId);
        if (typeof(this._components[id]) !== 'undefined') throw Error.invalidOperation(String.format(Sys.Res.appDuplicateComponent, id));
        this._components[id] = component;
    }
    function Sys$_Application$beginCreateComponents() {
        /// <summary locid="M:J#Sys.Application.beginCreateComponents" />
        if (arguments.length !== 0) throw Error.parameterCount();
        this._creatingComponents = true;
    }
    function Sys$_Application$dispose() {
        /// <summary locid="M:J#Sys.Application.dispose" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._disposing) {
            this._disposing = true;
            if (this._timerCookie) {
                window.clearTimeout(this._timerCookie);
                delete this._timerCookie;
            }
            if (this._endRequestHandler) {
                Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(this._endRequestHandler);
                delete this._endRequestHandler;
            }
            if (this._beginRequestHandler) {
                Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(this._beginRequestHandler);
                delete this._beginRequestHandler;
            }
            if (window.pageUnload) {
                window.pageUnload(this, Sys.EventArgs.Empty);
            }
            var unloadHandler = this.get_events().getHandler("unload");
            if (unloadHandler) {
                unloadHandler(this, Sys.EventArgs.Empty);
            }
            var disposableObjects = Array.clone(this._disposableObjects);
            for (var i = 0, l = disposableObjects.length; i < l; i++) {
                var object = disposableObjects[i];
                if (typeof(object) !== "undefined") {
                    object.dispose();
                }
            }
            Array.clear(this._disposableObjects);
            Sys.UI.DomEvent.removeHandler(window, "unload", this._unloadHandlerDelegate);
            if (Sys._ScriptLoader) {
                var sl = Sys._ScriptLoader.getInstance();
                if(sl) {
                    sl.dispose();
                }
            }
            Sys._Application.callBaseMethod(this, 'dispose');
        }
    }
    function Sys$_Application$disposeElement(element, childNodesOnly) {
        /// <summary locid="M:J#Sys._Application.disposeElement" />
        /// <param name="element"></param>
        /// <param name="childNodesOnly" type="Boolean"></param>
        var e = Function._validateParams(arguments, [
            {name: "element"},
            {name: "childNodesOnly", type: Boolean}
        ]);
        if (e) throw e;
        if (element.nodeType === 1) {
            var children = element.getElementsByTagName("*");
            for (var i = children.length - 1; i >= 0; i--) {
                this._disposeElementInternal(children[i]);
            }
            if (!childNodesOnly) {
                this._disposeElementInternal(element);
            }
        }
    }
    function Sys$_Application$endCreateComponents() {
        /// <summary locid="M:J#Sys.Application.endCreateComponents" />
        if (arguments.length !== 0) throw Error.parameterCount();
        var components = this._secondPassComponents;
        for (var i = 0, l = components.length; i < l; i++) {
            var component = components[i].component;
            Sys$Component$_setReferences(component, components[i].references);
            component.endUpdate();
        }
        this._secondPassComponents = [];
        this._creatingComponents = false;
    }
    function Sys$_Application$findComponent(id, parent) {
        /// <summary locid="M:J#Sys.Application.findComponent" />
        /// <param name="id" type="String"></param>
        /// <param name="parent" optional="true" mayBeNull="true"></param>
        /// <returns type="Sys.Component" mayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String},
            {name: "parent", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        return (parent ?
            ((Sys.IContainer.isInstanceOfType(parent)) ?
                parent.findComponent(id) :
                parent[id] || null) :
            Sys.Application._components[id] || null);
    }
    function Sys$_Application$getComponents() {
        /// <summary locid="M:J#Sys.Application.getComponents" />
        /// <returns type="Array" elementType="Sys.Component"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        var res = [];
        var components = this._components;
        for (var name in components) {
            res[res.length] = components[name];
        }
        return res;
    }
    function Sys$_Application$initialize() {
        /// <summary locid="M:J#Sys.Application.initialize" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if(!this.get_isInitialized() && !this._disposing) {
            Sys._Application.callBaseMethod(this, 'initialize');
            this._raiseInit();
            if (this.get_stateString) {
                if (Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    this._beginRequestHandler = Function.createDelegate(this, this._onPageRequestManagerBeginRequest);
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(this._beginRequestHandler);
                    this._endRequestHandler = Function.createDelegate(this, this._onPageRequestManagerEndRequest);
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(this._endRequestHandler);
                }
                var loadedEntry = this.get_stateString();
                if (loadedEntry !== this._currentEntry) {
                    this._navigate(loadedEntry);
                }
                else {
                    this._ensureHistory();
                }
            }
            this.raiseLoad();
        }
    }
    function Sys$_Application$notifyScriptLoaded() {
        /// <summary locid="M:J#Sys.Application.notifyScriptLoaded" />
        if (arguments.length !== 0) throw Error.parameterCount();
    }
    function Sys$_Application$registerDisposableObject(object) {
        /// <summary locid="M:J#Sys.Application.registerDisposableObject" />
        /// <param name="object" type="Sys.IDisposable"></param>
        var e = Function._validateParams(arguments, [
            {name: "object", type: Sys.IDisposable}
        ]);
        if (e) throw e;
        if (!this._disposing) {
            var objects = this._disposableObjects,
                i = objects.length;
            objects[i] = object;
            object.__msdisposeindex = i;
        }
    }
    function Sys$_Application$raiseLoad() {
        /// <summary locid="M:J#Sys.Application.raiseLoad" />
        if (arguments.length !== 0) throw Error.parameterCount();
        var h = this.get_events().getHandler("load");
        var args = new Sys.ApplicationLoadEventArgs(Array.clone(this._createdComponents), !!this._loaded);
        this._loaded = true;
        if (h) {
            h(this, args);
        }
        if (window.pageLoad) {
            window.pageLoad(this, args);
        }
        this._createdComponents = [];
    }
    function Sys$_Application$removeComponent(component) {
        /// <summary locid="M:J#Sys.Application.removeComponent" />
        /// <param name="component" type="Sys.Component"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Sys.Component}
        ]);
        if (e) throw e;
        var id = component.get_id();
        if (id) delete this._components[id];
    }
    function Sys$_Application$unregisterDisposableObject(object) {
        /// <summary locid="M:J#Sys.Application.unregisterDisposableObject" />
        /// <param name="object" type="Sys.IDisposable"></param>
        var e = Function._validateParams(arguments, [
            {name: "object", type: Sys.IDisposable}
        ]);
        if (e) throw e;
        if (!this._disposing) {
            var i = object.__msdisposeindex;
            if (typeof(i) === "number") {
                var disposableObjects = this._disposableObjects;
                delete disposableObjects[i];
                delete object.__msdisposeindex;
                if (++this._deleteCount > 1000) {
                    var newArray = [];
                    for (var j = 0, l = disposableObjects.length; j < l; j++) {
                        object = disposableObjects[j];
                        if (typeof(object) !== "undefined") {
                            object.__msdisposeindex = newArray.length;
                            newArray.push(object);
                        }
                    }
                    this._disposableObjects = newArray;
                    this._deleteCount = 0;
                }
            }
        }
    }
    function Sys$_Application$_addComponentToSecondPass(component, references) {
        this._secondPassComponents[this._secondPassComponents.length] = {component: component, references: references};
    }
    function Sys$_Application$_disposeComponents(list) {
        if (list) {
            for (var i = list.length - 1; i >= 0; i--) {
                var item = list[i];
                if (typeof(item.dispose) === "function") {
                    item.dispose();
                }
            }
        }
    }
    function Sys$_Application$_disposeElementInternal(element) {
        var d = element.dispose;
        if (d && typeof(d) === "function") {
            element.dispose();
        }
        else {
            var c = element.control;
            if (c && typeof(c.dispose) === "function") {
                c.dispose();
            }
        }
        var list = element._behaviors;
        if (list) {
            this._disposeComponents(list);
        }
        list = element._components;
        if (list) {
            this._disposeComponents(list);
            element._components = null;
        }
    }
    function Sys$_Application$_domReady() {
        var check, er, app = this;
        function init() { app.initialize(); }
        var onload = function() {
            Sys.UI.DomEvent.removeHandler(window, "load", onload);
            init();
        }
        Sys.UI.DomEvent.addHandler(window, "load", onload);
        
        if (document.addEventListener) {
            try {
                document.addEventListener("DOMContentLoaded", check = function() {
                    document.removeEventListener("DOMContentLoaded", check, false);
                    init();
                }, false);
            }
            catch (er) { }
        }
        else if (document.attachEvent) {
            if ((window == window.top) && document.documentElement.doScroll) {
                var timeout, el = document.createElement("div");
                check = function() {
                    try {
                        el.doScroll("left");
                    }
                    catch (er) {
                        timeout = window.setTimeout(check, 0);
                        return;
                    }
                    el = null;
                    init();
                }
                check();
            }
            else {
		document.attachEvent("onreadystatechange", check = function() {
                    if (document.readyState === "complete") {
                        document.detachEvent("onreadystatechange", check);
                        init();
                    }
                });
            }
        }
    }
    function Sys$_Application$_raiseInit() {
        var handler = this.get_events().getHandler("init");
        if (handler) {
            this.beginCreateComponents();
            handler(this, Sys.EventArgs.Empty);
            this.endCreateComponents();
        }
    }
    function Sys$_Application$_unloadHandler(event) {
        this.dispose();
    }
Sys._Application.prototype = {
    _creatingComponents: false,
    _disposing: false,
    _deleteCount: 0,
    get_isCreatingComponents: Sys$_Application$get_isCreatingComponents,
    get_isDisposing: Sys$_Application$get_isDisposing,
    add_init: Sys$_Application$add_init,
    remove_init: Sys$_Application$remove_init,
    add_load: Sys$_Application$add_load,
    remove_load: Sys$_Application$remove_load,
    add_unload: Sys$_Application$add_unload,
    remove_unload: Sys$_Application$remove_unload,
    addComponent: Sys$_Application$addComponent,
    beginCreateComponents: Sys$_Application$beginCreateComponents,
    dispose: Sys$_Application$dispose,
    disposeElement: Sys$_Application$disposeElement,
    endCreateComponents: Sys$_Application$endCreateComponents,
    findComponent: Sys$_Application$findComponent,
    getComponents: Sys$_Application$getComponents,
    initialize: Sys$_Application$initialize,
    notifyScriptLoaded: Sys$_Application$notifyScriptLoaded,
    registerDisposableObject: Sys$_Application$registerDisposableObject,
    raiseLoad: Sys$_Application$raiseLoad,
    removeComponent: Sys$_Application$removeComponent,
    unregisterDisposableObject: Sys$_Application$unregisterDisposableObject,
    _addComponentToSecondPass: Sys$_Application$_addComponentToSecondPass,
    _disposeComponents: Sys$_Application$_disposeComponents,
    _disposeElementInternal: Sys$_Application$_disposeElementInternal,
    _domReady: Sys$_Application$_domReady,
    _raiseInit: Sys$_Application$_raiseInit,
    _unloadHandler: Sys$_Application$_unloadHandler
}
Sys._Application.registerClass('Sys._Application', Sys.Component, Sys.IContainer);
Sys.Application = new Sys._Application();
var $find = Sys.Application.findComponent;
 
Sys.UI.Behavior = function Sys$UI$Behavior(element) {
    /// <summary locid="M:J#Sys.UI.Behavior.#ctor" />
    /// <param name="element" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    Sys.UI.Behavior.initializeBase(this);
    this._element = element;
    var behaviors = element._behaviors;
    if (!behaviors) {
        element._behaviors = [this];
    }
    else {
        behaviors[behaviors.length] = this;
    }
}
    function Sys$UI$Behavior$get_element() {
        /// <value domElement="true" locid="P:J#Sys.UI.Behavior.element"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._element;
    }
    function Sys$UI$Behavior$get_id() {
        /// <value type="String" locid="P:J#Sys.UI.Behavior.id"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        var baseId = Sys.UI.Behavior.callBaseMethod(this, 'get_id');
        if (baseId) return baseId;
        if (!this._element || !this._element.id) return '';
        return this._element.id + '$' + this.get_name();
    }
    function Sys$UI$Behavior$get_name() {
        /// <value type="String" locid="P:J#Sys.UI.Behavior.name"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._name) return this._name;
        var name = Object.getTypeName(this);
        var i = name.lastIndexOf('.');
        if (i !== -1) name = name.substr(i + 1);
        if (!this.get_isInitialized()) this._name = name;
        return name;
    }
    function Sys$UI$Behavior$set_name(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        if ((value === '') || (value.charAt(0) === ' ') || (value.charAt(value.length - 1) === ' '))
            throw Error.argument('value', Sys.Res.invalidId);
        if (typeof(this._element[value]) !== 'undefined')
            throw Error.invalidOperation(String.format(Sys.Res.behaviorDuplicateName, value));
        if (this.get_isInitialized()) throw Error.invalidOperation(Sys.Res.cantSetNameAfterInit);
        this._name = value;
    }
    function Sys$UI$Behavior$initialize() {
        Sys.UI.Behavior.callBaseMethod(this, 'initialize');
        var name = this.get_name();
        if (name) this._element[name] = this;
    }
    function Sys$UI$Behavior$dispose() {
        Sys.UI.Behavior.callBaseMethod(this, 'dispose');
        var e = this._element;
        if (e) {
            var name = this.get_name();
            if (name) {
                e[name] = null;
            }
            var behaviors = e._behaviors;
            Array.remove(behaviors, this);
            if (behaviors.length === 0) {
                e._behaviors = null;
            }
            delete this._element;
        }
    }
Sys.UI.Behavior.prototype = {
    _name: null,
    get_element: Sys$UI$Behavior$get_element,
    get_id: Sys$UI$Behavior$get_id,
    get_name: Sys$UI$Behavior$get_name,
    set_name: Sys$UI$Behavior$set_name,
    initialize: Sys$UI$Behavior$initialize,
    dispose: Sys$UI$Behavior$dispose
}
Sys.UI.Behavior.registerClass('Sys.UI.Behavior', Sys.Component);
Sys.UI.Behavior.getBehaviorByName = function Sys$UI$Behavior$getBehaviorByName(element, name) {
    /// <summary locid="M:J#Sys.UI.Behavior.getBehaviorByName" />
    /// <param name="element" domElement="true"></param>
    /// <param name="name" type="String"></param>
    /// <returns type="Sys.UI.Behavior" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "name", type: String}
    ]);
    if (e) throw e;
    var b = element[name];
    return (b && Sys.UI.Behavior.isInstanceOfType(b)) ? b : null;
}
Sys.UI.Behavior.getBehaviors = function Sys$UI$Behavior$getBehaviors(element) {
    /// <summary locid="M:J#Sys.UI.Behavior.getBehaviors" />
    /// <param name="element" domElement="true"></param>
    /// <returns type="Array" elementType="Sys.UI.Behavior"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    if (!element._behaviors) return [];
    return Array.clone(element._behaviors);
}
Sys.UI.Behavior.getBehaviorsByType = function Sys$UI$Behavior$getBehaviorsByType(element, type) {
    /// <summary locid="M:J#Sys.UI.Behavior.getBehaviorsByType" />
    /// <param name="element" domElement="true"></param>
    /// <param name="type" type="Type"></param>
    /// <returns type="Array" elementType="Sys.UI.Behavior"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "type", type: Type}
    ]);
    if (e) throw e;
    var behaviors = element._behaviors;
    var results = [];
    if (behaviors) {
        for (var i = 0, l = behaviors.length; i < l; i++) {
            if (type.isInstanceOfType(behaviors[i])) {
                results[results.length] = behaviors[i];
            }
        }
    }
    return results;
}
 
Sys.UI.VisibilityMode = function Sys$UI$VisibilityMode() {
    /// <summary locid="M:J#Sys.UI.VisibilityMode.#ctor" />
    /// <field name="hide" type="Number" integer="true" static="true" locid="F:J#Sys.UI.VisibilityMode.hide"></field>
    /// <field name="collapse" type="Number" integer="true" static="true" locid="F:J#Sys.UI.VisibilityMode.collapse"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
Sys.UI.VisibilityMode.prototype = {
    hide: 0,
    collapse: 1
}
Sys.UI.VisibilityMode.registerEnum("Sys.UI.VisibilityMode");
 
Sys.UI.Control = function Sys$UI$Control(element) {
    /// <summary locid="M:J#Sys.UI.Control.#ctor" />
    /// <param name="element" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    if (typeof(element.control) !== 'undefined') throw Error.invalidOperation(Sys.Res.controlAlreadyDefined);
    Sys.UI.Control.initializeBase(this);
    this._element = element;
    element.control = this;
    var role = this.get_role();
    if (role) {
        element.setAttribute("role", role);
    }
}
    function Sys$UI$Control$get_element() {
        /// <value domElement="true" locid="P:J#Sys.UI.Control.element"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._element;
    }
    function Sys$UI$Control$get_id() {
        /// <value type="String" locid="P:J#Sys.UI.Control.id"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._element) return '';
        return this._element.id;
    }
    function Sys$UI$Control$set_id(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        throw Error.invalidOperation(Sys.Res.cantSetId);
    }
    function Sys$UI$Control$get_parent() {
        /// <value type="Sys.UI.Control" locid="P:J#Sys.UI.Control.parent"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._parent) return this._parent;
        if (!this._element) return null;
        
        var parentElement = this._element.parentNode;
        while (parentElement) {
            if (parentElement.control) {
                return parentElement.control;
            }
            parentElement = parentElement.parentNode;
        }
        return null;
    }
    function Sys$UI$Control$set_parent(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.UI.Control}]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        var parents = [this];
        var current = value;
        while (current) {
            if (Array.contains(parents, current)) throw Error.invalidOperation(Sys.Res.circularParentChain);
            parents[parents.length] = current;
            current = current.get_parent();
        }
        this._parent = value;
    }
    function Sys$UI$Control$get_role() {
        /// <value type="String" locid="P:J#Sys.UI.Control.role"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return null;
    }
    function Sys$UI$Control$get_visibilityMode() {
        /// <value type="Sys.UI.VisibilityMode" locid="P:J#Sys.UI.Control.visibilityMode"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        return Sys.UI.DomElement.getVisibilityMode(this._element);
    }
    function Sys$UI$Control$set_visibilityMode(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.UI.VisibilityMode}]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        Sys.UI.DomElement.setVisibilityMode(this._element, value);
    }
    function Sys$UI$Control$get_visible() {
        /// <value type="Boolean" locid="P:J#Sys.UI.Control.visible"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        return Sys.UI.DomElement.getVisible(this._element);
    }
    function Sys$UI$Control$set_visible(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        Sys.UI.DomElement.setVisible(this._element, value)
    }
    function Sys$UI$Control$addCssClass(className) {
        /// <summary locid="M:J#Sys.UI.Control.addCssClass" />
        /// <param name="className" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "className", type: String}
        ]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        Sys.UI.DomElement.addCssClass(this._element, className);
    }
    function Sys$UI$Control$dispose() {
        Sys.UI.Control.callBaseMethod(this, 'dispose');
        if (this._element) {
            this._element.control = null;
            delete this._element;
        }
        if (this._parent) delete this._parent;
    }
    function Sys$UI$Control$onBubbleEvent(source, args) {
        /// <summary locid="M:J#Sys.UI.Control.onBubbleEvent" />
        /// <param name="source"></param>
        /// <param name="args" type="Sys.EventArgs"></param>
        /// <returns type="Boolean"></returns>
        var e = Function._validateParams(arguments, [
            {name: "source"},
            {name: "args", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        return false;
    }
    function Sys$UI$Control$raiseBubbleEvent(source, args) {
        /// <summary locid="M:J#Sys.UI.Control.raiseBubbleEvent" />
        /// <param name="source"></param>
        /// <param name="args" type="Sys.EventArgs"></param>
        var e = Function._validateParams(arguments, [
            {name: "source"},
            {name: "args", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        this._raiseBubbleEvent(source, args);
    }
    function Sys$UI$Control$_raiseBubbleEvent(source, args) {
        var currentTarget = this.get_parent();
        while (currentTarget) {
            if (currentTarget.onBubbleEvent(source, args)) {
                return;
            }
            currentTarget = currentTarget.get_parent();
        }
    }
    function Sys$UI$Control$removeCssClass(className) {
        /// <summary locid="M:J#Sys.UI.Control.removeCssClass" />
        /// <param name="className" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "className", type: String}
        ]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        Sys.UI.DomElement.removeCssClass(this._element, className);
    }
    function Sys$UI$Control$toggleCssClass(className) {
        /// <summary locid="M:J#Sys.UI.Control.toggleCssClass" />
        /// <param name="className" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "className", type: String}
        ]);
        if (e) throw e;
        if (!this._element) throw Error.invalidOperation(Sys.Res.cantBeCalledAfterDispose);
        Sys.UI.DomElement.toggleCssClass(this._element, className);
    }
Sys.UI.Control.prototype = {
    _parent: null,
    _visibilityMode: Sys.UI.VisibilityMode.hide,
    get_element: Sys$UI$Control$get_element,
    get_id: Sys$UI$Control$get_id,
    set_id: Sys$UI$Control$set_id,
    get_parent: Sys$UI$Control$get_parent,
    set_parent: Sys$UI$Control$set_parent,
    get_role: Sys$UI$Control$get_role,
    get_visibilityMode: Sys$UI$Control$get_visibilityMode,
    set_visibilityMode: Sys$UI$Control$set_visibilityMode,
    get_visible: Sys$UI$Control$get_visible,
    set_visible: Sys$UI$Control$set_visible,
    addCssClass: Sys$UI$Control$addCssClass,
    dispose: Sys$UI$Control$dispose,
    onBubbleEvent: Sys$UI$Control$onBubbleEvent,
    raiseBubbleEvent: Sys$UI$Control$raiseBubbleEvent,
    _raiseBubbleEvent: Sys$UI$Control$_raiseBubbleEvent,
    removeCssClass: Sys$UI$Control$removeCssClass,
    toggleCssClass: Sys$UI$Control$toggleCssClass
}
Sys.UI.Control.registerClass('Sys.UI.Control', Sys.Component);
Sys.HistoryEventArgs = function Sys$HistoryEventArgs(state) {
    /// <summary locid="M:J#Sys.HistoryEventArgs.#ctor" />
    /// <param name="state" type="Object"></param>
    var e = Function._validateParams(arguments, [
        {name: "state", type: Object}
    ]);
    if (e) throw e;
    Sys.HistoryEventArgs.initializeBase(this);
    this._state = state;
}
    function Sys$HistoryEventArgs$get_state() {
        /// <value type="Object" locid="P:J#Sys.HistoryEventArgs.state"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._state;
    }
Sys.HistoryEventArgs.prototype = {
    get_state: Sys$HistoryEventArgs$get_state
}
Sys.HistoryEventArgs.registerClass('Sys.HistoryEventArgs', Sys.EventArgs);
Sys.Application._appLoadHandler = null;
Sys.Application._beginRequestHandler = null;
Sys.Application._clientId = null;
Sys.Application._currentEntry = '';
Sys.Application._endRequestHandler = null;
Sys.Application._history = null;
Sys.Application._enableHistory = false;
Sys.Application._historyEnabledInScriptManager = false;
Sys.Application._historyFrame = null;
Sys.Application._historyInitialized = false;
Sys.Application._historyPointIsNew = false;
Sys.Application._ignoreTimer = false;
Sys.Application._initialState = null;
Sys.Application._state = {};
Sys.Application._timerCookie = 0;
Sys.Application._timerHandler = null;
Sys.Application._uniqueId = null;
Sys._Application.prototype.get_stateString = function Sys$_Application$get_stateString() {
    /// <summary locid="M:J#Sys._Application.get_stateString" />
    if (arguments.length !== 0) throw Error.parameterCount();
    var hash = null;
    
    if (Sys.Browser.agent === Sys.Browser.Firefox) {
        var href = window.location.href;
        var hashIndex = href.indexOf('#');
        if (hashIndex !== -1) {
            hash = href.substring(hashIndex + 1);
        }
        else {
            hash = "";
        }
        return hash;
    }
    else {
        hash = window.location.hash;
    }
    
    if ((hash.length > 0) && (hash.charAt(0) === '#')) {
        hash = hash.substring(1);
    }
    return hash;
};
Sys._Application.prototype.get_enableHistory = function Sys$_Application$get_enableHistory() {
    /// <summary locid="M:J#Sys._Application.get_enableHistory" />
    if (arguments.length !== 0) throw Error.parameterCount();
    return this._enableHistory;
};
Sys._Application.prototype.set_enableHistory = function Sys$_Application$set_enableHistory(value) {
    if (this._initialized && !this._initializing) {
        throw Error.invalidOperation(Sys.Res.historyCannotEnableHistory);
    }
    else if (this._historyEnabledInScriptManager && !value) {
        throw Error.invalidOperation(Sys.Res.invalidHistorySettingCombination);
    }
    this._enableHistory = value;
};
Sys._Application.prototype.add_navigate = function Sys$_Application$add_navigate(handler) {
    /// <summary locid="E:J#Sys.Application.navigate" />
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    this.get_events().addHandler("navigate", handler);
};
Sys._Application.prototype.remove_navigate = function Sys$_Application$remove_navigate(handler) {
    /// <summary locid="M:J#Sys._Application.remove_navigate" />
    /// <param name="handler" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "handler", type: Function}
    ]);
    if (e) throw e;
    this.get_events().removeHandler("navigate", handler);
};
Sys._Application.prototype.addHistoryPoint = function Sys$_Application$addHistoryPoint(state, title) {
    /// <summary locid="M:J#Sys.Application.addHistoryPoint" />
    /// <param name="state" type="Object"></param>
    /// <param name="title" type="String" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "state", type: Object},
        {name: "title", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    if (!this._enableHistory) throw Error.invalidOperation(Sys.Res.historyCannotAddHistoryPointWithHistoryDisabled);
    for (var n in state) {
        var v = state[n];
        var t = typeof(v);
        if ((v !== null) && ((t === 'object') || (t === 'function') || (t === 'undefined'))) {
            throw Error.argument('state', Sys.Res.stateMustBeStringDictionary);
        }
    }
    this._ensureHistory();
    var initialState = this._state;
    for (var key in state) {
        var value = state[key];
        if (value === null) {
            if (typeof(initialState[key]) !== 'undefined') {
                delete initialState[key];
            }
        }
        else {
            initialState[key] = value;
        }
    }
    var entry = this._serializeState(initialState);
    this._historyPointIsNew = true;
    this._setState(entry, title);
    this._raiseNavigate();
};
Sys._Application.prototype.setServerId = function Sys$_Application$setServerId(clientId, uniqueId) {
    /// <summary locid="M:J#Sys.Application.setServerId" />
    /// <param name="clientId" type="String"></param>
    /// <param name="uniqueId" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "clientId", type: String},
        {name: "uniqueId", type: String}
    ]);
    if (e) throw e;
    this._clientId = clientId;
    this._uniqueId = uniqueId;
};
Sys._Application.prototype.setServerState = function Sys$_Application$setServerState(value) {
    /// <summary locid="M:J#Sys.Application.setServerState" />
    /// <param name="value" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String}
    ]);
    if (e) throw e;
    this._ensureHistory();
    this._state.__s = value;
    this._updateHiddenField(value);
};
Sys._Application.prototype._deserializeState = function Sys$_Application$_deserializeState(entry) {
    var result = {};
    entry = entry || '';
    var serverSeparator = entry.indexOf('&&');
    if ((serverSeparator !== -1) && (serverSeparator + 2 < entry.length)) {
        result.__s = entry.substr(serverSeparator + 2);
        entry = entry.substr(0, serverSeparator);
    }
    var tokens = entry.split('&');
    for (var i = 0, l = tokens.length; i < l; i++) {
        var token = tokens[i];
        var equal = token.indexOf('=');
        if ((equal !== -1) && (equal + 1 < token.length)) {
            var name = token.substr(0, equal);
            var value = token.substr(equal + 1);
            result[name] = decodeURIComponent(value);
        }
    }
    return result;
};
Sys._Application.prototype._enableHistoryInScriptManager = function Sys$_Application$_enableHistoryInScriptManager() {
    this._enableHistory = true;
    this._historyEnabledInScriptManager = true;
};
Sys._Application.prototype._ensureHistory = function Sys$_Application$_ensureHistory() {
    if (!this._historyInitialized && this._enableHistory) {
        if ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.documentMode < 8)) {
            this._historyFrame = document.getElementById('__historyFrame');
            if (!this._historyFrame) throw Error.invalidOperation(Sys.Res.historyMissingFrame);
            this._ignoreIFrame = true;
        }
        this._timerHandler = Function.createDelegate(this, this._onIdle);
        this._timerCookie = window.setTimeout(this._timerHandler, 100);
        
        try {
            this._initialState = this._deserializeState(this.get_stateString());
        } catch(e) {}
        
        this._historyInitialized = true;
    }
};
Sys._Application.prototype._navigate = function Sys$_Application$_navigate(entry) {
    this._ensureHistory();
    var state = this._deserializeState(entry);
    
    if (this._uniqueId) {
        var oldServerEntry = this._state.__s || '';
        var newServerEntry = state.__s || '';
        if (newServerEntry !== oldServerEntry) {
            this._updateHiddenField(newServerEntry);
            __doPostBack(this._uniqueId, newServerEntry);
            this._state = state;
            return;
        }
    }
    this._setState(entry);
    this._state = state;
    this._raiseNavigate();
};
Sys._Application.prototype._onIdle = function Sys$_Application$_onIdle() {
    delete this._timerCookie;
    
    var entry = this.get_stateString();
    if (entry !== this._currentEntry) {
        if (!this._ignoreTimer) {
            this._historyPointIsNew = false;
            this._navigate(entry);
        }
    }
    else {
        this._ignoreTimer = false;
    }
    this._timerCookie = window.setTimeout(this._timerHandler, 100);
};
Sys._Application.prototype._onIFrameLoad = function Sys$_Application$_onIFrameLoad(entry) {
    this._ensureHistory();
    if (!this._ignoreIFrame) {
        this._historyPointIsNew = false;
        this._navigate(entry);
    }
    this._ignoreIFrame = false;
};
Sys._Application.prototype._onPageRequestManagerBeginRequest = function Sys$_Application$_onPageRequestManagerBeginRequest(sender, args) {
    this._ignoreTimer = true;
};
Sys._Application.prototype._onPageRequestManagerEndRequest = function Sys$_Application$_onPageRequestManagerEndRequest(sender, args) {
    var dataItem = args.get_dataItems()[this._clientId];
    var eventTarget = document.getElementById("__EVENTTARGET");
    if (eventTarget && eventTarget.value === this._uniqueId) {
        eventTarget.value = '';
    }
    if (typeof(dataItem) !== 'undefined') {
        this.setServerState(dataItem);
        this._historyPointIsNew = true;
    }
    else {
        this._ignoreTimer = false;
    }
    var entry = this._serializeState(this._state);
    if (entry !== this._currentEntry) {
        this._ignoreTimer = true;
        this._setState(entry);
        this._raiseNavigate();
    }
};
Sys._Application.prototype._raiseNavigate = function Sys$_Application$_raiseNavigate() {
    var h = this.get_events().getHandler("navigate");
    var stateClone = {};
    for (var key in this._state) {
        if (key !== '__s') {
            stateClone[key] = this._state[key];
        }
    }
    var args = new Sys.HistoryEventArgs(stateClone);
    if (h) {
        h(this, args);
    }
    var err;
    try {
        if ((Sys.Browser.agent === Sys.Browser.Firefox) && window.location.hash &&
            (!window.frameElement || window.top.location.hash)) {
            window.history.go(0);
        }
    }
    catch(err) {
    }
};
Sys._Application.prototype._serializeState = function Sys$_Application$_serializeState(state) {
    var serialized = [];
    for (var key in state) {
        var value = state[key];
        if (key === '__s') {
            var serverState = value;
        }
        else {
            if (key.indexOf('=') !== -1) throw Error.argument('state', Sys.Res.stateFieldNameInvalid);
            serialized[serialized.length] = key + '=' + encodeURIComponent(value);
        }
    }
    return serialized.join('&') + (serverState ? '&&' + serverState : '');
};
Sys._Application.prototype._setState = function Sys$_Application$_setState(entry, title) {
    if (this._enableHistory) {
        entry = entry || '';
        if (entry !== this._currentEntry) {
            if (window.theForm) {
                var action = window.theForm.action;
                var hashIndex = action.indexOf('#');
                window.theForm.action = ((hashIndex !== -1) ? action.substring(0, hashIndex) : action) + '#' + entry;
            }
        
            if (this._historyFrame && this._historyPointIsNew) {
                this._ignoreIFrame = true;
                var frameDoc = this._historyFrame.contentWindow.document;
                frameDoc.open("javascript:'<html></html>'");
                frameDoc.write("<html><head><title>" + (title || document.title) +
                    "</title><scri" + "pt type=\"text/javascript\">parent.Sys.Application._onIFrameLoad(" + 
                    Sys.Serialization.JavaScriptSerializer.serialize(entry) +
                    ");</scri" + "pt></head><body></body></html>");
                frameDoc.close();
            }
            this._ignoreTimer = false;
            this._currentEntry = entry;
            if (this._historyFrame || this._historyPointIsNew) {
                var currentHash = this.get_stateString();
                if (entry !== currentHash) {
                    var loc = document.location;
                    if (loc.href.length - loc.hash.length + entry.length > 1024) {
                        throw Error.invalidOperation(Sys.Res.urlMustBeLessThan1024chars);
                    }
                    window.location.hash = entry;
                    this._currentEntry = this.get_stateString();
                    if ((typeof(title) !== 'undefined') && (title !== null)) {
                        document.title = title;
                    }
                }
            }
            this._historyPointIsNew = false;
        }
    }
};
Sys._Application.prototype._updateHiddenField = function Sys$_Application$_updateHiddenField(value) {
    if (this._clientId) {
        var serverStateField = document.getElementById(this._clientId);
        if (serverStateField) {
            serverStateField.value = value;
        }
    }
};
 
if (!window.XMLHttpRequest) {
    window.XMLHttpRequest = function window$XMLHttpRequest() {
        var progIDs = [ 'Msxml2.XMLHTTP.3.0', 'Msxml2.XMLHTTP' ];
        for (var i = 0, l = progIDs.length; i < l; i++) {
            try {
                return new ActiveXObject(progIDs[i]);
            }
            catch (ex) {
            }
        }
        return null;
    }
}
Type.registerNamespace('Sys.Net');
 
Sys.Net.WebRequestExecutor = function Sys$Net$WebRequestExecutor() {
    /// <summary locid="M:J#Sys.Net.WebRequestExecutor.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    this._webRequest = null;
    this._resultObject = null;
}
    function Sys$Net$WebRequestExecutor$get_webRequest() {
        /// <value type="Sys.Net.WebRequest" locid="P:J#Sys.Net.WebRequestExecutor.webRequest"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._webRequest;
    }
    function Sys$Net$WebRequestExecutor$_set_webRequest(value) {
        if (this.get_started()) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOnceStarted, 'set_webRequest'));
        }
        this._webRequest = value;
    }
    function Sys$Net$WebRequestExecutor$get_started() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebRequestExecutor.started"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_responseAvailable() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebRequestExecutor.responseAvailable"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_timedOut() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebRequestExecutor.timedOut"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_aborted() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebRequestExecutor.aborted"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_responseData() {
        /// <value type="String" locid="P:J#Sys.Net.WebRequestExecutor.responseData"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_statusCode() {
        /// <value type="Number" locid="P:J#Sys.Net.WebRequestExecutor.statusCode"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_statusText() {
        /// <value type="String" locid="P:J#Sys.Net.WebRequestExecutor.statusText"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_xml() {
        /// <value locid="P:J#Sys.Net.WebRequestExecutor.xml"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$get_object() {
        /// <value locid="P:J#Sys.Net.WebRequestExecutor.object"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._resultObject) {
            this._resultObject = Sys.Serialization.JavaScriptSerializer.deserialize(this.get_responseData());
        }
        return this._resultObject;
    }
    function Sys$Net$WebRequestExecutor$executeRequest() {
        /// <summary locid="M:J#Sys.Net.WebRequestExecutor.executeRequest" />
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$abort() {
        /// <summary locid="M:J#Sys.Net.WebRequestExecutor.abort" />
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$getResponseHeader(header) {
        /// <summary locid="M:J#Sys.Net.WebRequestExecutor.getResponseHeader" />
        /// <param name="header" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "header", type: String}
        ]);
        if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$Net$WebRequestExecutor$getAllResponseHeaders() {
        /// <summary locid="M:J#Sys.Net.WebRequestExecutor.getAllResponseHeaders" />
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
Sys.Net.WebRequestExecutor.prototype = {
    get_webRequest: Sys$Net$WebRequestExecutor$get_webRequest,
    _set_webRequest: Sys$Net$WebRequestExecutor$_set_webRequest,
    get_started: Sys$Net$WebRequestExecutor$get_started,
    get_responseAvailable: Sys$Net$WebRequestExecutor$get_responseAvailable,
    get_timedOut: Sys$Net$WebRequestExecutor$get_timedOut,
    get_aborted: Sys$Net$WebRequestExecutor$get_aborted,
    get_responseData: Sys$Net$WebRequestExecutor$get_responseData,
    get_statusCode: Sys$Net$WebRequestExecutor$get_statusCode,
    get_statusText: Sys$Net$WebRequestExecutor$get_statusText,
    get_xml: Sys$Net$WebRequestExecutor$get_xml,
    get_object: Sys$Net$WebRequestExecutor$get_object,
    executeRequest: Sys$Net$WebRequestExecutor$executeRequest,
    abort: Sys$Net$WebRequestExecutor$abort,
    getResponseHeader: Sys$Net$WebRequestExecutor$getResponseHeader,
    getAllResponseHeaders: Sys$Net$WebRequestExecutor$getAllResponseHeaders
}
Sys.Net.WebRequestExecutor.registerClass('Sys.Net.WebRequestExecutor');
 
Sys.Net.XMLDOM = function Sys$Net$XMLDOM(markup) {
    /// <summary locid="M:J#Sys.Net.XMLDOM.#ctor" />
    /// <param name="markup" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "markup", type: String}
    ]);
    if (e) throw e;
    if (!window.DOMParser) {
        var progIDs = [ 'Msxml2.DOMDocument.3.0', 'Msxml2.DOMDocument' ];
        for (var i = 0, l = progIDs.length; i < l; i++) {
            try {
                var xmlDOM = new ActiveXObject(progIDs[i]);
                xmlDOM.async = false;
                xmlDOM.loadXML(markup);
                xmlDOM.setProperty('SelectionLanguage', 'XPath');
                return xmlDOM;
            }
            catch (ex) {
            }
        }
    }
    else {
        try {
            var domParser = new window.DOMParser();
            return domParser.parseFromString(markup, 'text/xml');
        }
        catch (ex) {
        }
    }
    return null;
}
Sys.Net.XMLHttpExecutor = function Sys$Net$XMLHttpExecutor() {
    /// <summary locid="M:J#Sys.Net.XMLHttpExecutor.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    Sys.Net.XMLHttpExecutor.initializeBase(this);
    var _this = this;
    this._xmlHttpRequest = null;
    this._webRequest = null;
    this._responseAvailable = false;
    this._timedOut = false;
    this._timer = null;
    this._aborted = false;
    this._started = false;
    this._onReadyStateChange = (function () {
        
        if (_this._xmlHttpRequest.readyState === 4 ) {
            try {
                if (typeof(_this._xmlHttpRequest.status) === "undefined") {
                    return;
                }
            }
            catch(ex) {
                return;
            }
            
            _this._clearTimer();
            _this._responseAvailable = true;
                _this._webRequest.completed(Sys.EventArgs.Empty);
                if (_this._xmlHttpRequest != null) {
                    _this._xmlHttpRequest.onreadystatechange = Function.emptyMethod;
                    _this._xmlHttpRequest = null;
                }
        }
    });
    this._clearTimer = (function() {
        if (_this._timer != null) {
            window.clearTimeout(_this._timer);
            _this._timer = null;
        }
    });
    this._onTimeout = (function() {
        if (!_this._responseAvailable) {
            _this._clearTimer();
            _this._timedOut = true;
            _this._xmlHttpRequest.onreadystatechange = Function.emptyMethod;
            _this._xmlHttpRequest.abort();
            _this._webRequest.completed(Sys.EventArgs.Empty);
            _this._xmlHttpRequest = null;
        }
    });
}
    function Sys$Net$XMLHttpExecutor$get_timedOut() {
        /// <value type="Boolean" locid="P:J#Sys.Net.XMLHttpExecutor.timedOut"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._timedOut;
    }
    function Sys$Net$XMLHttpExecutor$get_started() {
        /// <value type="Boolean" locid="P:J#Sys.Net.XMLHttpExecutor.started"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._started;
    }
    function Sys$Net$XMLHttpExecutor$get_responseAvailable() {
        /// <value type="Boolean" locid="P:J#Sys.Net.XMLHttpExecutor.responseAvailable"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._responseAvailable;
    }
    function Sys$Net$XMLHttpExecutor$get_aborted() {
        /// <value type="Boolean" locid="P:J#Sys.Net.XMLHttpExecutor.aborted"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._aborted;
    }
    function Sys$Net$XMLHttpExecutor$executeRequest() {
        /// <summary locid="M:J#Sys.Net.XMLHttpExecutor.executeRequest" />
        if (arguments.length !== 0) throw Error.parameterCount();
        this._webRequest = this.get_webRequest();
        if (this._started) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOnceStarted, 'executeRequest'));
        }
        if (this._webRequest === null) {
            throw Error.invalidOperation(Sys.Res.nullWebRequest);
        }
        var body = this._webRequest.get_body();
        var headers = this._webRequest.get_headers();
        this._xmlHttpRequest = new XMLHttpRequest();
        this._xmlHttpRequest.onreadystatechange = this._onReadyStateChange;
        var verb = this._webRequest.get_httpVerb();
        this._xmlHttpRequest.open(verb, this._webRequest.getResolvedUrl(), true );
        this._xmlHttpRequest.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        if (headers) {
            for (var header in headers) {
                var val = headers[header];
                if (typeof(val) !== "function")
                    this._xmlHttpRequest.setRequestHeader(header, val);
            }
        }
        if (verb.toLowerCase() === "post") {
            if ((headers === null) || !headers['Content-Type']) {
                this._xmlHttpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
            }
            if (!body) {
                body = "";
            }
        }
        var timeout = this._webRequest.get_timeout();
        if (timeout > 0) {
            this._timer = window.setTimeout(Function.createDelegate(this, this._onTimeout), timeout);
        }
        this._xmlHttpRequest.send(body);
        this._started = true;
    }
    function Sys$Net$XMLHttpExecutor$getResponseHeader(header) {
        /// <summary locid="M:J#Sys.Net.XMLHttpExecutor.getResponseHeader" />
        /// <param name="header" type="String"></param>
        /// <returns type="String"></returns>
        var e = Function._validateParams(arguments, [
            {name: "header", type: String}
        ]);
        if (e) throw e;
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'getResponseHeader'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'getResponseHeader'));
        }
        var result;
        try {
            result = this._xmlHttpRequest.getResponseHeader(header);
        } catch (e) {
        }
        if (!result) result = "";
        return result;
    }
    function Sys$Net$XMLHttpExecutor$getAllResponseHeaders() {
        /// <summary locid="M:J#Sys.Net.XMLHttpExecutor.getAllResponseHeaders" />
        /// <returns type="String"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'getAllResponseHeaders'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'getAllResponseHeaders'));
        }
        return this._xmlHttpRequest.getAllResponseHeaders();
    }
    function Sys$Net$XMLHttpExecutor$get_responseData() {
        /// <value type="String" locid="P:J#Sys.Net.XMLHttpExecutor.responseData"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'get_responseData'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'get_responseData'));
        }
        return this._xmlHttpRequest.responseText;
    }
    function Sys$Net$XMLHttpExecutor$get_statusCode() {
        /// <value type="Number" locid="P:J#Sys.Net.XMLHttpExecutor.statusCode"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'get_statusCode'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'get_statusCode'));
        }
        var result = 0;
        try {
            result = this._xmlHttpRequest.status;
        }
        catch(ex) {
        }
        return result;
    }
    function Sys$Net$XMLHttpExecutor$get_statusText() {
        /// <value type="String" locid="P:J#Sys.Net.XMLHttpExecutor.statusText"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'get_statusText'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'get_statusText'));
        }
        return this._xmlHttpRequest.statusText;
    }
    function Sys$Net$XMLHttpExecutor$get_xml() {
        /// <value locid="P:J#Sys.Net.XMLHttpExecutor.xml"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._responseAvailable) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallBeforeResponse, 'get_xml'));
        }
        if (!this._xmlHttpRequest) {
            throw Error.invalidOperation(String.format(Sys.Res.cannotCallOutsideHandler, 'get_xml'));
        }
        var xml = this._xmlHttpRequest.responseXML;
        if (!xml || !xml.documentElement) {
            xml = Sys.Net.XMLDOM(this._xmlHttpRequest.responseText);
            if (!xml || !xml.documentElement)
                return null;
        }
        else if (navigator.userAgent.indexOf('MSIE') !== -1) {
            xml.setProperty('SelectionLanguage', 'XPath');
        }
        if (xml.documentElement.namespaceURI === "http://www.mozilla.org/newlayout/xml/parsererror.xml" &&
            xml.documentElement.tagName === "parsererror") {
            return null;
        }
        
        if (xml.documentElement.firstChild && xml.documentElement.firstChild.tagName === "parsererror") {
            return null;
        }
        
        return xml;
    }
    function Sys$Net$XMLHttpExecutor$abort() {
        /// <summary locid="M:J#Sys.Net.XMLHttpExecutor.abort" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._started) {
            throw Error.invalidOperation(Sys.Res.cannotAbortBeforeStart);
        }
        if (this._aborted || this._responseAvailable || this._timedOut)
            return;
        this._aborted = true;
        this._clearTimer();
        if (this._xmlHttpRequest && !this._responseAvailable) {
            this._xmlHttpRequest.onreadystatechange = Function.emptyMethod;
            this._xmlHttpRequest.abort();
            
            this._xmlHttpRequest = null;            
            this._webRequest.completed(Sys.EventArgs.Empty);
        }
    }
Sys.Net.XMLHttpExecutor.prototype = {
    get_timedOut: Sys$Net$XMLHttpExecutor$get_timedOut,
    get_started: Sys$Net$XMLHttpExecutor$get_started,
    get_responseAvailable: Sys$Net$XMLHttpExecutor$get_responseAvailable,
    get_aborted: Sys$Net$XMLHttpExecutor$get_aborted,
    executeRequest: Sys$Net$XMLHttpExecutor$executeRequest,
    getResponseHeader: Sys$Net$XMLHttpExecutor$getResponseHeader,
    getAllResponseHeaders: Sys$Net$XMLHttpExecutor$getAllResponseHeaders,
    get_responseData: Sys$Net$XMLHttpExecutor$get_responseData,
    get_statusCode: Sys$Net$XMLHttpExecutor$get_statusCode,
    get_statusText: Sys$Net$XMLHttpExecutor$get_statusText,
    get_xml: Sys$Net$XMLHttpExecutor$get_xml,
    abort: Sys$Net$XMLHttpExecutor$abort
}
Sys.Net.XMLHttpExecutor.registerClass('Sys.Net.XMLHttpExecutor', Sys.Net.WebRequestExecutor);
 
Sys.Net._WebRequestManager = function Sys$Net$_WebRequestManager() {
    /// <summary locid="P:J#Sys.Net.WebRequestManager.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    this._defaultTimeout = 0;
    this._defaultExecutorType = "Sys.Net.XMLHttpExecutor";
}
    function Sys$Net$_WebRequestManager$add_invokingRequest(handler) {
        /// <summary locid="E:J#Sys.Net.WebRequestManager.invokingRequest" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this._get_eventHandlerList().addHandler("invokingRequest", handler);
    }
    function Sys$Net$_WebRequestManager$remove_invokingRequest(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this._get_eventHandlerList().removeHandler("invokingRequest", handler);
    }
    function Sys$Net$_WebRequestManager$add_completedRequest(handler) {
        /// <summary locid="E:J#Sys.Net.WebRequestManager.completedRequest" />
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this._get_eventHandlerList().addHandler("completedRequest", handler);
    }
    function Sys$Net$_WebRequestManager$remove_completedRequest(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this._get_eventHandlerList().removeHandler("completedRequest", handler);
    }
    function Sys$Net$_WebRequestManager$_get_eventHandlerList() {
        if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Net$_WebRequestManager$get_defaultTimeout() {
        /// <value type="Number" locid="P:J#Sys.Net.WebRequestManager.defaultTimeout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._defaultTimeout;
    }
    function Sys$Net$_WebRequestManager$set_defaultTimeout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if (value < 0) {
            throw Error.argumentOutOfRange("value", value, Sys.Res.invalidTimeout);
        }
        this._defaultTimeout = value;
    }
    function Sys$Net$_WebRequestManager$get_defaultExecutorType() {
        /// <value type="String" locid="P:J#Sys.Net.WebRequestManager.defaultExecutorType"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._defaultExecutorType;
    }
    function Sys$Net$_WebRequestManager$set_defaultExecutorType(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._defaultExecutorType = value;
    }
    function Sys$Net$_WebRequestManager$executeRequest(webRequest) {
        /// <summary locid="M:J#Sys.Net.WebRequestManager.executeRequest" />
        /// <param name="webRequest" type="Sys.Net.WebRequest"></param>
        var e = Function._validateParams(arguments, [
            {name: "webRequest", type: Sys.Net.WebRequest}
        ]);
        if (e) throw e;
        var executor = webRequest.get_executor();
        if (!executor) {
            var failed = false;
            try {
                var executorType = eval(this._defaultExecutorType);
                executor = new executorType();
            } catch (e) {
                failed = true;
            }
            if (failed  || !Sys.Net.WebRequestExecutor.isInstanceOfType(executor) || !executor) {
                throw Error.argument("defaultExecutorType", String.format(Sys.Res.invalidExecutorType, this._defaultExecutorType));
            }
            webRequest.set_executor(executor);
        }
        if (executor.get_aborted()) {
            return;
        }
        var evArgs = new Sys.Net.NetworkRequestEventArgs(webRequest);
        var handler = this._get_eventHandlerList().getHandler("invokingRequest");
        if (handler) {
            handler(this, evArgs);
        }
        if (!evArgs.get_cancel()) {
            executor.executeRequest();
        }
    }
Sys.Net._WebRequestManager.prototype = {
    add_invokingRequest: Sys$Net$_WebRequestManager$add_invokingRequest,
    remove_invokingRequest: Sys$Net$_WebRequestManager$remove_invokingRequest,
    add_completedRequest: Sys$Net$_WebRequestManager$add_completedRequest,
    remove_completedRequest: Sys$Net$_WebRequestManager$remove_completedRequest,
    _get_eventHandlerList: Sys$Net$_WebRequestManager$_get_eventHandlerList,
    get_defaultTimeout: Sys$Net$_WebRequestManager$get_defaultTimeout,
    set_defaultTimeout: Sys$Net$_WebRequestManager$set_defaultTimeout,
    get_defaultExecutorType: Sys$Net$_WebRequestManager$get_defaultExecutorType,
    set_defaultExecutorType: Sys$Net$_WebRequestManager$set_defaultExecutorType,
    executeRequest: Sys$Net$_WebRequestManager$executeRequest
}
Sys.Net._WebRequestManager.registerClass('Sys.Net._WebRequestManager');
Sys.Net.WebRequestManager = new Sys.Net._WebRequestManager();
 
Sys.Net.NetworkRequestEventArgs = function Sys$Net$NetworkRequestEventArgs(webRequest) {
    /// <summary locid="M:J#Sys.Net.NetworkRequestEventArgs.#ctor" />
    /// <param name="webRequest" type="Sys.Net.WebRequest"></param>
    var e = Function._validateParams(arguments, [
        {name: "webRequest", type: Sys.Net.WebRequest}
    ]);
    if (e) throw e;
    Sys.Net.NetworkRequestEventArgs.initializeBase(this);
    this._webRequest = webRequest;
}
    function Sys$Net$NetworkRequestEventArgs$get_webRequest() {
        /// <value type="Sys.Net.WebRequest" locid="P:J#Sys.Net.NetworkRequestEventArgs.webRequest"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._webRequest;
    }
Sys.Net.NetworkRequestEventArgs.prototype = {
    get_webRequest: Sys$Net$NetworkRequestEventArgs$get_webRequest
}
Sys.Net.NetworkRequestEventArgs.registerClass('Sys.Net.NetworkRequestEventArgs', Sys.CancelEventArgs);
 
Sys.Net.WebRequest = function Sys$Net$WebRequest() {
    /// <summary locid="M:J#Sys.Net.WebRequest.#ctor" />
    if (arguments.length !== 0) throw Error.parameterCount();
    this._url = "";
    this._headers = { };
    this._body = null;
    this._userContext = null;
    this._httpVerb = null;
    this._executor = null;
    this._invokeCalled = false;
    this._timeout = 0;
}
    function Sys$Net$WebRequest$add_completed(handler) {
    /// <summary locid="E:J#Sys.Net.WebRequest.completed" />
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        this._get_eventHandlerList().addHandler("completed", handler);
    }
    function Sys$Net$WebRequest$remove_completed(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        this._get_eventHandlerList().removeHandler("completed", handler);
    }
    function Sys$Net$WebRequest$completed(eventArgs) {
        /// <summary locid="M:J#Sys.Net.WebRequest.completed" />
        /// <param name="eventArgs" type="Sys.EventArgs"></param>
        var e = Function._validateParams(arguments, [
            {name: "eventArgs", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        var handler = Sys.Net.WebRequestManager._get_eventHandlerList().getHandler("completedRequest");
        if (handler) {
            handler(this._executor, eventArgs);
        }
        handler = this._get_eventHandlerList().getHandler("completed");
        if (handler) {
            handler(this._executor, eventArgs);
        }
    }
    function Sys$Net$WebRequest$_get_eventHandlerList() {
        if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Net$WebRequest$get_url() {
        /// <value type="String" locid="P:J#Sys.Net.WebRequest.url"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._url;
    }
    function Sys$Net$WebRequest$set_url(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._url = value;
    }
    function Sys$Net$WebRequest$get_headers() {
        /// <value locid="P:J#Sys.Net.WebRequest.headers"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._headers;
    }
    function Sys$Net$WebRequest$get_httpVerb() {
        /// <value type="String" locid="P:J#Sys.Net.WebRequest.httpVerb"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._httpVerb === null) {
            if (this._body === null) {
                return "GET";
            }
            return "POST";
        }
        return this._httpVerb;
    }
    function Sys$Net$WebRequest$set_httpVerb(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        if (value.length === 0) {
            throw Error.argument('value', Sys.Res.invalidHttpVerb);
        }
        this._httpVerb = value;
    }
    function Sys$Net$WebRequest$get_body() {
        /// <value mayBeNull="true" locid="P:J#Sys.Net.WebRequest.body"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._body;
    }
    function Sys$Net$WebRequest$set_body(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._body = value;
    }
    function Sys$Net$WebRequest$get_userContext() {
        /// <value mayBeNull="true" locid="P:J#Sys.Net.WebRequest.userContext"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._userContext;
    }
    function Sys$Net$WebRequest$set_userContext(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._userContext = value;
    }
    function Sys$Net$WebRequest$get_executor() {
        /// <value type="Sys.Net.WebRequestExecutor" locid="P:J#Sys.Net.WebRequest.executor"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._executor;
    }
    function Sys$Net$WebRequest$set_executor(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.Net.WebRequestExecutor}]);
        if (e) throw e;
        if (this._executor !== null && this._executor.get_started()) {
            throw Error.invalidOperation(Sys.Res.setExecutorAfterActive);
        }
        this._executor = value;
        this._executor._set_webRequest(this);
    }
    function Sys$Net$WebRequest$get_timeout() {
        /// <value type="Number" locid="P:J#Sys.Net.WebRequest.timeout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._timeout === 0) {
            return Sys.Net.WebRequestManager.get_defaultTimeout();
        }
        return this._timeout;
    }
    function Sys$Net$WebRequest$set_timeout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if (value < 0) {
            throw Error.argumentOutOfRange("value", value, Sys.Res.invalidTimeout);
        }
        this._timeout = value;
    }
    function Sys$Net$WebRequest$getResolvedUrl() {
        /// <summary locid="M:J#Sys.Net.WebRequest.getResolvedUrl" />
        /// <returns type="String"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        return Sys.Net.WebRequest._resolveUrl(this._url);
    }
    function Sys$Net$WebRequest$invoke() {
        /// <summary locid="M:J#Sys.Net.WebRequest.invoke" />
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._invokeCalled) {
            throw Error.invalidOperation(Sys.Res.invokeCalledTwice);
        }
        Sys.Net.WebRequestManager.executeRequest(this);
        this._invokeCalled = true;
    }
Sys.Net.WebRequest.prototype = {
    add_completed: Sys$Net$WebRequest$add_completed,
    remove_completed: Sys$Net$WebRequest$remove_completed,
    completed: Sys$Net$WebRequest$completed,
    _get_eventHandlerList: Sys$Net$WebRequest$_get_eventHandlerList,
    get_url: Sys$Net$WebRequest$get_url,
    set_url: Sys$Net$WebRequest$set_url,
    get_headers: Sys$Net$WebRequest$get_headers,
    get_httpVerb: Sys$Net$WebRequest$get_httpVerb,
    set_httpVerb: Sys$Net$WebRequest$set_httpVerb,
    get_body: Sys$Net$WebRequest$get_body,
    set_body: Sys$Net$WebRequest$set_body,
    get_userContext: Sys$Net$WebRequest$get_userContext,
    set_userContext: Sys$Net$WebRequest$set_userContext,
    get_executor: Sys$Net$WebRequest$get_executor,
    set_executor: Sys$Net$WebRequest$set_executor,
    get_timeout: Sys$Net$WebRequest$get_timeout,
    set_timeout: Sys$Net$WebRequest$set_timeout,
    getResolvedUrl: Sys$Net$WebRequest$getResolvedUrl,
    invoke: Sys$Net$WebRequest$invoke
}
Sys.Net.WebRequest._resolveUrl = function Sys$Net$WebRequest$_resolveUrl(url, baseUrl) {
    if (url && url.indexOf('://') !== -1) {
        return url;
    }
    if (!baseUrl || baseUrl.length === 0) {
        var baseElement = document.getElementsByTagName('base')[0];
        if (baseElement && baseElement.href && baseElement.href.length > 0) {
            baseUrl = baseElement.href;
        }
        else {
            baseUrl = document.URL;
        }
    }
    var qsStart = baseUrl.indexOf('?');
    if (qsStart !== -1) {
        baseUrl = baseUrl.substr(0, qsStart);
    }
    qsStart = baseUrl.indexOf('#');
    if (qsStart !== -1) {
        baseUrl = baseUrl.substr(0, qsStart);
    }
    baseUrl = baseUrl.substr(0, baseUrl.lastIndexOf('/') + 1);
    if (!url || url.length === 0) {
        return baseUrl;
    }
    if (url.charAt(0) === '/') {
        var slashslash = baseUrl.indexOf('://');
        if (slashslash === -1) {
            throw Error.argument("baseUrl", Sys.Res.badBaseUrl1);
        }
        var nextSlash = baseUrl.indexOf('/', slashslash + 3);
        if (nextSlash === -1) {
            throw Error.argument("baseUrl", Sys.Res.badBaseUrl2);
        }
        return baseUrl.substr(0, nextSlash) + url;
    }
    else {
        var lastSlash = baseUrl.lastIndexOf('/');
        if (lastSlash === -1) {
            throw Error.argument("baseUrl", Sys.Res.badBaseUrl3);
        }
        return baseUrl.substr(0, lastSlash+1) + url;
    }
}
Sys.Net.WebRequest._createQueryString = function Sys$Net$WebRequest$_createQueryString(queryString, encodeMethod, addParams) {
    encodeMethod = encodeMethod || encodeURIComponent;
    var i = 0, obj, val, arg, sb = new Sys.StringBuilder();
    if (queryString) {
        for (arg in queryString) {
            obj = queryString[arg];
            if (typeof(obj) === "function") continue;
            val = Sys.Serialization.JavaScriptSerializer.serialize(obj);
            if (i++) {
                sb.append('&');
            }
            sb.append(arg);
            sb.append('=');
            sb.append(encodeMethod(val));
        }
    }
    if (addParams) {
        if (i) {
            sb.append('&');
        }
        sb.append(addParams);
    }
    return sb.toString();
}
Sys.Net.WebRequest._createUrl = function Sys$Net$WebRequest$_createUrl(url, queryString, addParams) {
    if (!queryString && !addParams) {
        return url;
    }
    var qs = Sys.Net.WebRequest._createQueryString(queryString, null, addParams);
    return qs.length
        ? url + ((url && url.indexOf('?') >= 0) ? "&" : "?") + qs
        : url;
}
Sys.Net.WebRequest.registerClass('Sys.Net.WebRequest');
 
Sys._ScriptLoaderTask = function Sys$_ScriptLoaderTask(scriptElement, completedCallback) {
    /// <summary locid="M:J#Sys._ScriptLoaderTask.#ctor" />
    /// <param name="scriptElement" domElement="true"></param>
    /// <param name="completedCallback" type="Function"></param>
    var e = Function._validateParams(arguments, [
        {name: "scriptElement", domElement: true},
        {name: "completedCallback", type: Function}
    ]);
    if (e) throw e;
    this._scriptElement = scriptElement;
    this._completedCallback = completedCallback;
}
    function Sys$_ScriptLoaderTask$get_scriptElement() {
        /// <value domElement="true" locid="P:J#Sys._ScriptLoaderTask.scriptElement"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._scriptElement;
    }
    function Sys$_ScriptLoaderTask$dispose() {
        if(this._disposed) {
            return;
        }
        this._disposed = true;
        this._removeScriptElementHandlers();
        Sys._ScriptLoaderTask._clearScript(this._scriptElement);
        this._scriptElement = null;
    }
    function Sys$_ScriptLoaderTask$execute() {
        /// <summary locid="M:J#Sys._ScriptLoaderTask.execute" />
        if (arguments.length !== 0) throw Error.parameterCount();
        this._addScriptElementHandlers();
        var headElements = document.getElementsByTagName('head');
        if (headElements.length === 0) {
             throw new Error.invalidOperation(Sys.Res.scriptLoadFailedNoHead);
        }
        else {
             headElements[0].appendChild(this._scriptElement);
        }
    }
    function Sys$_ScriptLoaderTask$_addScriptElementHandlers() {
        this._scriptLoadDelegate = Function.createDelegate(this, this._scriptLoadHandler);
        
        if (Sys.Browser.agent !== Sys.Browser.InternetExplorer) {
            this._scriptElement.readyState = 'loaded';
            $addHandler(this._scriptElement, 'load', this._scriptLoadDelegate);
        }
        else {
            $addHandler(this._scriptElement, 'readystatechange', this._scriptLoadDelegate);
        }    
        if (this._scriptElement.addEventListener) {
            this._scriptErrorDelegate = Function.createDelegate(this, this._scriptErrorHandler);
            this._scriptElement.addEventListener('error', this._scriptErrorDelegate, false);
        }
    }
    function Sys$_ScriptLoaderTask$_removeScriptElementHandlers() {
        if(this._scriptLoadDelegate) {
            var scriptElement = this.get_scriptElement();
            if (Sys.Browser.agent !== Sys.Browser.InternetExplorer) {
                $removeHandler(scriptElement, 'load', this._scriptLoadDelegate);
            }
            else {
                $removeHandler(scriptElement, 'readystatechange', this._scriptLoadDelegate);
            }
            if (this._scriptErrorDelegate) {
                this._scriptElement.removeEventListener('error', this._scriptErrorDelegate, false);
                this._scriptErrorDelegate = null;
            }
            this._scriptLoadDelegate = null;
        }
    }
    function Sys$_ScriptLoaderTask$_scriptErrorHandler() {
        if(this._disposed) {
            return;
        }
        
        this._completedCallback(this.get_scriptElement(), false);
    }
    function Sys$_ScriptLoaderTask$_scriptLoadHandler() {
        if(this._disposed) {
            return;
        }
        var scriptElement = this.get_scriptElement();
        if ((scriptElement.readyState !== 'loaded') &&
            (scriptElement.readyState !== 'complete')) {
            return;
        }
        
        this._completedCallback(scriptElement, true);
    }
Sys._ScriptLoaderTask.prototype = {
    get_scriptElement: Sys$_ScriptLoaderTask$get_scriptElement,
    dispose: Sys$_ScriptLoaderTask$dispose,
    execute: Sys$_ScriptLoaderTask$execute,
    _addScriptElementHandlers: Sys$_ScriptLoaderTask$_addScriptElementHandlers,    
    _removeScriptElementHandlers: Sys$_ScriptLoaderTask$_removeScriptElementHandlers,    
    _scriptErrorHandler: Sys$_ScriptLoaderTask$_scriptErrorHandler,
    _scriptLoadHandler: Sys$_ScriptLoaderTask$_scriptLoadHandler  
}
Sys._ScriptLoaderTask.registerClass("Sys._ScriptLoaderTask", null, Sys.IDisposable);
Sys._ScriptLoaderTask._clearScript = function Sys$_ScriptLoaderTask$_clearScript(scriptElement) {
    if (!Sys.Debug.isDebug) {
        scriptElement.parentNode.removeChild(scriptElement);
    }
}
Type.registerNamespace('Sys.Net');
 
Sys.Net.WebServiceProxy = function Sys$Net$WebServiceProxy() {
}
    function Sys$Net$WebServiceProxy$get_timeout() {
        /// <value type="Number" locid="P:J#Sys.Net.WebServiceProxy.timeout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._timeout || 0;
    }
    function Sys$Net$WebServiceProxy$set_timeout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if (value < 0) { throw Error.argumentOutOfRange('value', value, Sys.Res.invalidTimeout); }
        this._timeout = value;
    }
    function Sys$Net$WebServiceProxy$get_defaultUserContext() {
        /// <value mayBeNull="true" locid="P:J#Sys.Net.WebServiceProxy.defaultUserContext"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return (typeof(this._userContext) === "undefined") ? null : this._userContext;
    }
    function Sys$Net$WebServiceProxy$set_defaultUserContext(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._userContext = value;
    }
    function Sys$Net$WebServiceProxy$get_defaultSucceededCallback() {
        /// <value type="Function" mayBeNull="true" locid="P:J#Sys.Net.WebServiceProxy.defaultSucceededCallback"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._succeeded || null;
    }
    function Sys$Net$WebServiceProxy$set_defaultSucceededCallback(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Function, mayBeNull: true}]);
        if (e) throw e;
        this._succeeded = value;
    }
    function Sys$Net$WebServiceProxy$get_defaultFailedCallback() {
        /// <value type="Function" mayBeNull="true" locid="P:J#Sys.Net.WebServiceProxy.defaultFailedCallback"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._failed || null;
    }
    function Sys$Net$WebServiceProxy$set_defaultFailedCallback(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Function, mayBeNull: true}]);
        if (e) throw e;
        this._failed = value;
    }
    function Sys$Net$WebServiceProxy$get_enableJsonp() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebServiceProxy.enableJsonp"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return !!this._jsonp;
    }
    function Sys$Net$WebServiceProxy$set_enableJsonp(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._jsonp = value;
    }
    function Sys$Net$WebServiceProxy$get_path() {
        /// <value type="String" locid="P:J#Sys.Net.WebServiceProxy.path"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._path || null;
    }
    function Sys$Net$WebServiceProxy$set_path(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._path = value;
    }
    function Sys$Net$WebServiceProxy$get_jsonpCallbackParameter() {
        /// <value type="String" locid="P:J#Sys.Net.WebServiceProxy.jsonpCallbackParameter"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._callbackParameter || "callback";
    }
    function Sys$Net$WebServiceProxy$set_jsonpCallbackParameter(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._callbackParameter = value;
    }
    function Sys$Net$WebServiceProxy$_invoke(servicePath, methodName, useGet, params, onSuccess, onFailure, userContext) {
        /// <summary locid="M:J#Sys.Net.WebServiceProxy._invoke" />
        /// <param name="servicePath" type="String"></param>
        /// <param name="methodName" type="String"></param>
        /// <param name="useGet" type="Boolean"></param>
        /// <param name="params"></param>
        /// <param name="onSuccess" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="onFailure" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <returns type="Sys.Net.WebRequest" mayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            {name: "servicePath", type: String},
            {name: "methodName", type: String},
            {name: "useGet", type: Boolean},
            {name: "params"},
            {name: "onSuccess", type: Function, mayBeNull: true, optional: true},
            {name: "onFailure", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        onSuccess = onSuccess || this.get_defaultSucceededCallback();
        onFailure = onFailure || this.get_defaultFailedCallback();
        if (userContext === null || typeof userContext === 'undefined') userContext = this.get_defaultUserContext();
        return Sys.Net.WebServiceProxy.invoke(servicePath, methodName, useGet, params, onSuccess, onFailure, userContext, this.get_timeout(), this.get_enableJsonp(), this.get_jsonpCallbackParameter());
    }
Sys.Net.WebServiceProxy.prototype = {
    get_timeout: Sys$Net$WebServiceProxy$get_timeout,
    set_timeout: Sys$Net$WebServiceProxy$set_timeout,
    get_defaultUserContext: Sys$Net$WebServiceProxy$get_defaultUserContext,
    set_defaultUserContext: Sys$Net$WebServiceProxy$set_defaultUserContext,
    get_defaultSucceededCallback: Sys$Net$WebServiceProxy$get_defaultSucceededCallback,
    set_defaultSucceededCallback: Sys$Net$WebServiceProxy$set_defaultSucceededCallback,
    get_defaultFailedCallback: Sys$Net$WebServiceProxy$get_defaultFailedCallback,
    set_defaultFailedCallback: Sys$Net$WebServiceProxy$set_defaultFailedCallback,
    get_enableJsonp: Sys$Net$WebServiceProxy$get_enableJsonp,
    set_enableJsonp: Sys$Net$WebServiceProxy$set_enableJsonp,
    get_path: Sys$Net$WebServiceProxy$get_path,
    set_path: Sys$Net$WebServiceProxy$set_path,
    get_jsonpCallbackParameter: Sys$Net$WebServiceProxy$get_jsonpCallbackParameter,
    set_jsonpCallbackParameter: Sys$Net$WebServiceProxy$set_jsonpCallbackParameter,
    _invoke: Sys$Net$WebServiceProxy$_invoke
}
Sys.Net.WebServiceProxy.registerClass('Sys.Net.WebServiceProxy');
Sys.Net.WebServiceProxy.invoke = function Sys$Net$WebServiceProxy$invoke(servicePath, methodName, useGet, params, onSuccess, onFailure, userContext, timeout, enableJsonp, jsonpCallbackParameter) {
    /// <summary locid="M:J#Sys.Net.WebServiceProxy.invoke" />
    /// <param name="servicePath" type="String"></param>
    /// <param name="methodName" type="String" mayBeNull="true" optional="true"></param>
    /// <param name="useGet" type="Boolean" optional="true"></param>
    /// <param name="params" mayBeNull="true" optional="true"></param>
    /// <param name="onSuccess" type="Function" mayBeNull="true" optional="true"></param>
    /// <param name="onFailure" type="Function" mayBeNull="true" optional="true"></param>
    /// <param name="userContext" mayBeNull="true" optional="true"></param>
    /// <param name="timeout" type="Number" optional="true"></param>
    /// <param name="enableJsonp" type="Boolean" optional="true" mayBeNull="true"></param>
    /// <param name="jsonpCallbackParameter" type="String" optional="true" mayBeNull="true"></param>
    /// <returns type="Sys.Net.WebRequest" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "servicePath", type: String},
        {name: "methodName", type: String, mayBeNull: true, optional: true},
        {name: "useGet", type: Boolean, optional: true},
        {name: "params", mayBeNull: true, optional: true},
        {name: "onSuccess", type: Function, mayBeNull: true, optional: true},
        {name: "onFailure", type: Function, mayBeNull: true, optional: true},
        {name: "userContext", mayBeNull: true, optional: true},
        {name: "timeout", type: Number, optional: true},
        {name: "enableJsonp", type: Boolean, mayBeNull: true, optional: true},
        {name: "jsonpCallbackParameter", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var schemeHost = (enableJsonp !== false) ? Sys.Net.WebServiceProxy._xdomain.exec(servicePath) : null,
        tempCallback, jsonp = schemeHost && (schemeHost.length === 3) && 
            ((schemeHost[1] !== location.protocol) || (schemeHost[2] !== location.host));
    useGet = jsonp || useGet;
    if (jsonp) {
        jsonpCallbackParameter = jsonpCallbackParameter || "callback";
        tempCallback = "_jsonp" + Sys._jsonp++;
    }
    if (!params) params = {};
    var urlParams = params;
    if (!useGet || !urlParams) urlParams = {};
    var script, error, timeoutcookie = null, loader, body = null,
        url = Sys.Net.WebRequest._createUrl(methodName
            ? (servicePath+"/"+encodeURIComponent(methodName))
            : servicePath, urlParams, jsonp ? (jsonpCallbackParameter + "=Sys." + tempCallback) : null);
    if (jsonp) {
        script = document.createElement("script");
        script.src = url;
        loader = new Sys._ScriptLoaderTask(script, function(script, loaded) {
            if (!loaded || tempCallback) {
                jsonpComplete({ Message: String.format(Sys.Res.webServiceFailedNoMsg, methodName) }, -1);
            }
        });
        function jsonpComplete(data, statusCode) {
            if (timeoutcookie !== null) {
                window.clearTimeout(timeoutcookie);
                timeoutcookie = null;
            }
            loader.dispose();
            delete Sys[tempCallback];
            tempCallback = null; 
            if ((typeof(statusCode) !== "undefined") && (statusCode !== 200)) {
                if (onFailure) {
                    error = new Sys.Net.WebServiceError(false,
                            data.Message || String.format(Sys.Res.webServiceFailedNoMsg, methodName),
                            data.StackTrace || null,
                            data.ExceptionType || null,
                            data);
                    error._statusCode = statusCode;
                    onFailure(error, userContext, methodName);
                }
                else {
                    if (data.StackTrace && data.Message) {
                        error = data.StackTrace + "-- " + data.Message;
                    }
                    else {
                        error = data.StackTrace || data.Message;
                    }
                    error = String.format(error ? Sys.Res.webServiceFailed : Sys.Res.webServiceFailedNoMsg, methodName, error);
                    throw Sys.Net.WebServiceProxy._createFailedError(methodName, String.format(Sys.Res.webServiceFailed, methodName, error));
                }
            }
            else if (onSuccess) {
                onSuccess(data, userContext, methodName);
            }
        }
        Sys[tempCallback] = jsonpComplete;
        loader.execute();
        return null;
    }
    var request = new Sys.Net.WebRequest();
    request.set_url(url);
    request.get_headers()['Content-Type'] = 'application/json; charset=utf-8';
    if (!useGet) {
        body = Sys.Serialization.JavaScriptSerializer.serialize(params);
        if (body === "{}") body = "";
    }
    request.set_body(body);
    request.add_completed(onComplete);
    if (timeout && timeout > 0) request.set_timeout(timeout);
    request.invoke();
    
    function onComplete(response, eventArgs) {
        if (response.get_responseAvailable()) {
            var statusCode = response.get_statusCode();
            var result = null;
           
            try {
                var contentType = response.getResponseHeader("Content-Type");
                if (contentType.startsWith("application/json")) {
                    result = response.get_object();
                }
                else if (contentType.startsWith("text/xml")) {
                    result = response.get_xml();
                }
                else {
                    result = response.get_responseData();
                }
            } catch (ex) {
            }
            var error = response.getResponseHeader("jsonerror");
            var errorObj = (error === "true");
            if (errorObj) {
                if (result) {
                    result = new Sys.Net.WebServiceError(false, result.Message, result.StackTrace, result.ExceptionType, result);
                }
            }
            else if (contentType.startsWith("application/json")) {
                result = (!result || (typeof(result.d) === "undefined")) ? result : result.d;
            }
            if (((statusCode < 200) || (statusCode >= 300)) || errorObj) {
                if (onFailure) {
                    if (!result || !errorObj) {
                        result = new Sys.Net.WebServiceError(false , String.format(Sys.Res.webServiceFailedNoMsg, methodName));
                    }
                    result._statusCode = statusCode;
                    onFailure(result, userContext, methodName);
                }
                else {
                    if (result && errorObj) {
                        error = result.get_exceptionType() + "-- " + result.get_message();
                    }
                    else {
                        error = response.get_responseData();
                    }
                    throw Sys.Net.WebServiceProxy._createFailedError(methodName, String.format(Sys.Res.webServiceFailed, methodName, error));
                }
            }
            else if (onSuccess) {
                onSuccess(result, userContext, methodName);
            }
        }
        else {
            var msg;
            if (response.get_timedOut()) {
                msg = String.format(Sys.Res.webServiceTimedOut, methodName);
            }
            else {
                msg = String.format(Sys.Res.webServiceFailedNoMsg, methodName)
            }
            if (onFailure) {
                onFailure(new Sys.Net.WebServiceError(response.get_timedOut(), msg, "", ""), userContext, methodName);
            }
            else {
                throw Sys.Net.WebServiceProxy._createFailedError(methodName, msg);
            }
        }
    }
    return request;
}
Sys.Net.WebServiceProxy._createFailedError = function Sys$Net$WebServiceProxy$_createFailedError(methodName, errorMessage) {
    var displayMessage = "Sys.Net.WebServiceFailedException: " + errorMessage;
    var e = Error.create(displayMessage, { 'name': 'Sys.Net.WebServiceFailedException', 'methodName': methodName });
    e.popStackFrame();
    return e;
}
Sys.Net.WebServiceProxy._defaultFailedCallback = function Sys$Net$WebServiceProxy$_defaultFailedCallback(err, methodName) {
    var error = err.get_exceptionType() + "-- " + err.get_message();
    throw Sys.Net.WebServiceProxy._createFailedError(methodName, String.format(Sys.Res.webServiceFailed, methodName, error));
}
Sys.Net.WebServiceProxy._generateTypedConstructor = function Sys$Net$WebServiceProxy$_generateTypedConstructor(type) {
    return function(properties) {
        if (properties) {
            for (var name in properties) {
                this[name] = properties[name];
            }
        }
        this.__type = type;
    }
}
Sys._jsonp = 0;
Sys.Net.WebServiceProxy._xdomain = /^\s*([a-zA-Z0-9\+\-\.]+\:)\/\/([^?#\/]+)/;
 
Sys.Net.WebServiceError = function Sys$Net$WebServiceError(timedOut, message, stackTrace, exceptionType, errorObject) {
    /// <summary locid="M:J#Sys.Net.WebServiceError.#ctor" />
    /// <param name="timedOut" type="Boolean"></param>
    /// <param name="message" type="String" mayBeNull="true"></param>
    /// <param name="stackTrace" type="String" mayBeNull="true" optional="true"></param>
    /// <param name="exceptionType" type="String" mayBeNull="true" optional="true"></param>
    /// <param name="errorObject" type="Object" mayBeNull="true" optional="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "timedOut", type: Boolean},
        {name: "message", type: String, mayBeNull: true},
        {name: "stackTrace", type: String, mayBeNull: true, optional: true},
        {name: "exceptionType", type: String, mayBeNull: true, optional: true},
        {name: "errorObject", type: Object, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    this._timedOut = timedOut;
    this._message = message;
    this._stackTrace = stackTrace;
    this._exceptionType = exceptionType;
    this._errorObject = errorObject;
    this._statusCode = -1;
}
    function Sys$Net$WebServiceError$get_timedOut() {
        /// <value type="Boolean" locid="P:J#Sys.Net.WebServiceError.timedOut"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._timedOut;
    }
    function Sys$Net$WebServiceError$get_statusCode() {
        /// <value type="Number" locid="P:J#Sys.Net.WebServiceError.statusCode"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._statusCode;
    }
    function Sys$Net$WebServiceError$get_message() {
        /// <value type="String" locid="P:J#Sys.Net.WebServiceError.message"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._message;
    }
    function Sys$Net$WebServiceError$get_stackTrace() {
        /// <value type="String" locid="P:J#Sys.Net.WebServiceError.stackTrace"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._stackTrace || "";
    }
    function Sys$Net$WebServiceError$get_exceptionType() {
        /// <value type="String" locid="P:J#Sys.Net.WebServiceError.exceptionType"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._exceptionType || "";
    }
    function Sys$Net$WebServiceError$get_errorObject() {
        /// <value type="Object" locid="P:J#Sys.Net.WebServiceError.errorObject"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._errorObject || null;
    }
Sys.Net.WebServiceError.prototype = {
    get_timedOut: Sys$Net$WebServiceError$get_timedOut,
    get_statusCode: Sys$Net$WebServiceError$get_statusCode,
    get_message: Sys$Net$WebServiceError$get_message,
    get_stackTrace: Sys$Net$WebServiceError$get_stackTrace,
    get_exceptionType: Sys$Net$WebServiceError$get_exceptionType,
    get_errorObject: Sys$Net$WebServiceError$get_errorObject
}
Sys.Net.WebServiceError.registerClass('Sys.Net.WebServiceError');


Type.registerNamespace('Sys');

Sys.Res={
'urlMustBeLessThan1024chars':'The history state must be small enough to not make the url larger than 1024 characters.',
'argumentTypeName':'Value is not the name of an existing type.',
'cantBeCalledAfterDispose':'Can\'t be called after dispose.',
'componentCantSetIdAfterAddedToApp':'The id property of a component can\'t be set after it\'s been added to the Application object.',
'behaviorDuplicateName':'A behavior with name \'{0}\' already exists or it is the name of an existing property on the target element.',
'notATypeName':'Value is not a valid type name.',
'elementNotFound':'An element with id \'{0}\' could not be found.',
'stateMustBeStringDictionary':'The state object can only have null and string fields.',
'boolTrueOrFalse':'Value must be \'true\' or \'false\'.',
'scriptLoadFailedNoHead':'ScriptLoader requires pages to contain a <head> element.',
'stringFormatInvalid':'The format string is invalid.',
'referenceNotFound':'Component \'{0}\' was not found.',
'enumReservedName':'\'{0}\' is a reserved name that can\'t be used as an enum value name.',
'circularParentChain':'The chain of control parents can\'t have circular references.',
'namespaceContainsNonObject':'Object {0} already exists and is not an object.',
'undefinedEvent':'\'{0}\' is not an event.',
'propertyUndefined':'\'{0}\' is not a property or an existing field.',
'observableConflict':'Object already contains a member with the name \'{0}\'.',
'historyCannotEnableHistory':'Cannot set enableHistory after initialization.',
'eventHandlerInvalid':'Handler was not added through the Sys.UI.DomEvent.addHandler method.',
'scriptLoadFailedDebug':'The script \'{0}\' failed to load. Check for:\r\n Inaccessible path.\r\n Script errors. (IE) Enable \'Display a notification about every script error\' under advanced settings.',
'propertyNotWritable':'\'{0}\' is not a writable property.',
'enumInvalidValueName':'\'{0}\' is not a valid name for an enum value.',
'controlAlreadyDefined':'A control is already associated with the element.',
'addHandlerCantBeUsedForError':'Can\'t add a handler for the error event using this method. Please set the window.onerror property instead.',
'cantAddNonFunctionhandler':'Can\'t add a handler that is not a function.',
'invalidNameSpace':'Value is not a valid namespace identifier.',
'notAnInterface':'Value is not a valid interface.',
'eventHandlerNotFunction':'Handler must be a function.',
'propertyNotAnArray':'\'{0}\' is not an Array property.',
'namespaceContainsClass':'Object {0} already exists as a class, enum, or interface.',
'typeRegisteredTwice':'Type {0} has already been registered. The type may be defined multiple times or the script file that defines it may have already been loaded. A possible cause is a change of settings during a partial update.',
'cantSetNameAfterInit':'The name property can\'t be set on this object after initialization.',
'historyMissingFrame':'For the history feature to work in IE, the page must have an iFrame element with id \'__historyFrame\' pointed to a page that gets its title from the \'title\' query string parameter and calls Sys.Application._onIFrameLoad() on the parent window. This can be done by setting EnableHistory to true on ScriptManager.',
'appDuplicateComponent':'Two components with the same id \'{0}\' can\'t be added to the application.',
'historyCannotAddHistoryPointWithHistoryDisabled':'A history point can only be added if enableHistory is set to true.',
'baseNotAClass':'Value is not a class.',
'expectedElementOrId':'Value must be a DOM element or DOM element Id.',
'methodNotFound':'No method found with name \'{0}\'.',
'arrayParseBadFormat':'Value must be a valid string representation for an array. It must start with a \'[\' and end with a \']\'.',
'stateFieldNameInvalid':'State field names must not contain any \'=\' characters.',
'cantSetId':'The id property can\'t be set on this object.',
'stringFormatBraceMismatch':'The format string contains an unmatched opening or closing brace.',
'enumValueNotInteger':'An enumeration definition can only contain integer values.',
'propertyNullOrUndefined':'Cannot set the properties of \'{0}\' because it returned a null value.',
'argumentDomNode':'Value must be a DOM element or a text node.',
'componentCantSetIdTwice':'The id property of a component can\'t be set more than once.',
'createComponentOnDom':'Value must be null for Components that are not Controls or Behaviors.',
'createNotComponent':'{0} does not derive from Sys.Component.',
'createNoDom':'Value must not be null for Controls and Behaviors.',
'cantAddWithoutId':'Can\'t add a component that doesn\'t have an id.',
'notObservable':'Instances of type \'{0}\' cannot be observed.',
'badTypeName':'Value is not the name of the type being registered or the name is a reserved word.',
'argumentInteger':'Value must be an integer.',
'invokeCalledTwice':'Cannot call invoke more than once.',
'webServiceFailed':'The server method \'{0}\' failed with the following error: {1}',
'argumentType':'Object cannot be converted to the required type.',
'argumentNull':'Value cannot be null.',
'scriptAlreadyLoaded':'The script \'{0}\' has been referenced multiple times. If referencing Microsoft AJAX scripts explicitly, set the MicrosoftAjaxMode property of the ScriptManager to Explicit.',
'scriptDependencyNotFound':'The script \'{0}\' failed to load because it is dependent on script \'{1}\'.',
'formatBadFormatSpecifier':'Format specifier was invalid.',
'requiredScriptReferenceNotIncluded':'\'{0}\' requires that you have included a script reference to \'{1}\'.',
'webServiceFailedNoMsg':'The server method \'{0}\' failed.',
'argumentDomElement':'Value must be a DOM element.',
'invalidExecutorType':'Could not create a valid Sys.Net.WebRequestExecutor from: {0}.',
'cannotCallBeforeResponse':'Cannot call {0} when responseAvailable is false.',
'actualValue':'Actual value was {0}.',
'enumInvalidValue':'\'{0}\' is not a valid value for enum {1}.',
'scriptLoadFailed':'The script \'{0}\' could not be loaded.',
'parameterCount':'Parameter count mismatch.',
'cannotDeserializeEmptyString':'Cannot deserialize empty string.',
'formatInvalidString':'Input string was not in a correct format.',
'invalidTimeout':'Value must be greater than or equal to zero.',
'cannotAbortBeforeStart':'Cannot abort when executor has not started.',
'argument':'Value does not fall within the expected range.',
'cannotDeserializeInvalidJson':'Cannot deserialize. The data does not correspond to valid JSON.',
'invalidHttpVerb':'httpVerb cannot be set to an empty or null string.',
'nullWebRequest':'Cannot call executeRequest with a null webRequest.',
'eventHandlerInvalid':'Handler was not added through the Sys.UI.DomEvent.addHandler method.',
'cannotSerializeNonFiniteNumbers':'Cannot serialize non finite numbers.',
'argumentUndefined':'Value cannot be undefined.',
'webServiceInvalidReturnType':'The server method \'{0}\' returned an invalid type. Expected type: {1}',
'servicePathNotSet':'The path to the web service has not been set.',
'argumentTypeWithTypes':'Object of type \'{0}\' cannot be converted to type \'{1}\'.',
'cannotCallOnceStarted':'Cannot call {0} once started.',
'badBaseUrl1':'Base URL does not contain ://.',
'badBaseUrl2':'Base URL does not contain another /.',
'badBaseUrl3':'Cannot find last / in base URL.',
'setExecutorAfterActive':'Cannot set executor after it has become active.',
'paramName':'Parameter name: {0}',
'nullReferenceInPath':'Null reference while evaluating data path: \'{0}\'.',
'cannotCallOutsideHandler':'Cannot call {0} outside of a completed event handler.',
'cannotSerializeObjectWithCycle':'Cannot serialize object with cyclic reference within child properties.',
'format':'One of the identified items was in an invalid format.',
'assertFailedCaller':'Assertion Failed: {0}\r\nat {1}',
'argumentOutOfRange':'Specified argument was out of the range of valid values.',
'webServiceTimedOut':'The server method \'{0}\' timed out.',
'notImplemented':'The method or operation is not implemented.',
'assertFailed':'Assertion Failed: {0}',
'invalidOperation':'Operation is not valid due to the current state of the object.',
'breakIntoDebugger':'{0}\r\n\r\nBreak into debugger?'
};

//!----------------------------------------------------------
//! Copyright (C) Microsoft Corporation. All rights reserved.
//!----------------------------------------------------------
//! MicrosoftMvcValidation.js


Type.registerNamespace('Sys.Mvc');

////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.Validation

Sys.Mvc.$create_Validation = function Sys_Mvc_Validation() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.JsonValidationField

Sys.Mvc.$create_JsonValidationField = function Sys_Mvc_JsonValidationField() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.JsonValidationOptions

Sys.Mvc.$create_JsonValidationOptions = function Sys_Mvc_JsonValidationOptions() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.JsonValidationRule

Sys.Mvc.$create_JsonValidationRule = function Sys_Mvc_JsonValidationRule() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.ValidationContext

Sys.Mvc.$create_ValidationContext = function Sys_Mvc_ValidationContext() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.NumberValidator

Sys.Mvc.NumberValidator = function Sys_Mvc_NumberValidator() {
}
Sys.Mvc.NumberValidator.create = function Sys_Mvc_NumberValidator$create(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    return Function.createDelegate(new Sys.Mvc.NumberValidator(), new Sys.Mvc.NumberValidator().validate);
}
Sys.Mvc.NumberValidator.prototype = {
    
    validate: function Sys_Mvc_NumberValidator$validate(value, context) {
        /// <param name="value" type="String">
        /// </param>
        /// <param name="context" type="Sys.Mvc.ValidationContext">
        /// </param>
        /// <returns type="Object"></returns>
        if (Sys.Mvc._validationUtil.stringIsNullOrEmpty(value)) {
            return true;
        }
        var n = Number.parseLocale(value);
        return (!isNaN(n));
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.FormContext

Sys.Mvc.FormContext = function Sys_Mvc_FormContext(formElement, validationSummaryElement) {
    /// <param name="formElement" type="Object" domElement="true">
    /// </param>
    /// <param name="validationSummaryElement" type="Object" domElement="true">
    /// </param>
    /// <field name="_validationSummaryErrorCss" type="String" static="true">
    /// </field>
    /// <field name="_validationSummaryValidCss" type="String" static="true">
    /// </field>
    /// <field name="_formValidationTag" type="String" static="true">
    /// </field>
    /// <field name="_onClickHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_onSubmitHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_errors" type="Array">
    /// </field>
    /// <field name="_submitButtonClicked" type="Object" domElement="true">
    /// </field>
    /// <field name="_validationSummaryElement" type="Object" domElement="true">
    /// </field>
    /// <field name="_validationSummaryULElement" type="Object" domElement="true">
    /// </field>
    /// <field name="fields" type="Array" elementType="FieldContext">
    /// </field>
    /// <field name="_formElement" type="Object" domElement="true">
    /// </field>
    /// <field name="replaceValidationSummary" type="Boolean">
    /// </field>
    this._errors = [];
    this.fields = new Array(0);
    this._formElement = formElement;
    this._validationSummaryElement = validationSummaryElement;
    formElement[Sys.Mvc.FormContext._formValidationTag] = this;
    if (validationSummaryElement) {
        var ulElements = validationSummaryElement.getElementsByTagName('ul');
        if (ulElements.length > 0) {
            this._validationSummaryULElement = ulElements[0];
        }
    }
    this._onClickHandler = Function.createDelegate(this, this._form_OnClick);
    this._onSubmitHandler = Function.createDelegate(this, this._form_OnSubmit);
}
Sys.Mvc.FormContext._Application_Load = function Sys_Mvc_FormContext$_Application_Load() {
    var allFormOptions = window.mvcClientValidationMetadata;
    if (allFormOptions) {
        while (allFormOptions.length > 0) {
            var thisFormOptions = allFormOptions.pop();
            Sys.Mvc.FormContext._parseJsonOptions(thisFormOptions);
        }
    }
}
Sys.Mvc.FormContext._getFormElementsWithName = function Sys_Mvc_FormContext$_getFormElementsWithName(formElement, name) {
    /// <param name="formElement" type="Object" domElement="true">
    /// </param>
    /// <param name="name" type="String">
    /// </param>
    /// <returns type="Array" elementType="Object" elementDomElement="true"></returns>
    var allElementsWithNameInForm = [];
    var allElementsWithName = document.getElementsByName(name);
    for (var i = 0; i < allElementsWithName.length; i++) {
        var thisElement = allElementsWithName[i];
        if (Sys.Mvc.FormContext._isElementInHierarchy(formElement, thisElement)) {
            Array.add(allElementsWithNameInForm, thisElement);
        }
    }
    return allElementsWithNameInForm;
}
Sys.Mvc.FormContext.getValidationForForm = function Sys_Mvc_FormContext$getValidationForForm(formElement) {
    /// <param name="formElement" type="Object" domElement="true">
    /// </param>
    /// <returns type="Sys.Mvc.FormContext"></returns>
    return formElement[Sys.Mvc.FormContext._formValidationTag];
}
Sys.Mvc.FormContext._isElementInHierarchy = function Sys_Mvc_FormContext$_isElementInHierarchy(parent, child) {
    /// <param name="parent" type="Object" domElement="true">
    /// </param>
    /// <param name="child" type="Object" domElement="true">
    /// </param>
    /// <returns type="Boolean"></returns>
    while (child) {
        if (parent === child) {
            return true;
        }
        child = child.parentNode;
    }
    return false;
}
Sys.Mvc.FormContext._parseJsonOptions = function Sys_Mvc_FormContext$_parseJsonOptions(options) {
    /// <param name="options" type="Sys.Mvc.JsonValidationOptions">
    /// </param>
    /// <returns type="Sys.Mvc.FormContext"></returns>
    var formElement = $get(options.FormId);
    var validationSummaryElement = (!Sys.Mvc._validationUtil.stringIsNullOrEmpty(options.ValidationSummaryId)) ? $get(options.ValidationSummaryId) : null;
    var formContext = new Sys.Mvc.FormContext(formElement, validationSummaryElement);
    formContext.enableDynamicValidation();
    formContext.replaceValidationSummary = options.ReplaceValidationSummary;
    for (var i = 0; i < options.Fields.length; i++) {
        var field = options.Fields[i];
        var fieldElements = Sys.Mvc.FormContext._getFormElementsWithName(formElement, field.FieldName);
        var validationMessageElement = (!Sys.Mvc._validationUtil.stringIsNullOrEmpty(field.ValidationMessageId)) ? $get(field.ValidationMessageId) : null;
        var fieldContext = new Sys.Mvc.FieldContext(formContext);
        Array.addRange(fieldContext.elements, fieldElements);
        fieldContext.validationMessageElement = validationMessageElement;
        fieldContext.replaceValidationMessageContents = field.ReplaceValidationMessageContents;
        for (var j = 0; j < field.ValidationRules.length; j++) {
            var rule = field.ValidationRules[j];
            var validator = Sys.Mvc.ValidatorRegistry.getValidator(rule);
            if (validator) {
                var validation = Sys.Mvc.$create_Validation();
                validation.fieldErrorMessage = rule.ErrorMessage;
                validation.validator = validator;
                Array.add(fieldContext.validations, validation);
            }
        }
        fieldContext.enableDynamicValidation();
        Array.add(formContext.fields, fieldContext);
    }
    var registeredValidatorCallbacks = formElement.validationCallbacks;
    if (!registeredValidatorCallbacks) {
        registeredValidatorCallbacks = [];
        formElement.validationCallbacks = registeredValidatorCallbacks;
    }
    registeredValidatorCallbacks.push(Function.createDelegate(null, function() {
        return Sys.Mvc._validationUtil.arrayIsNullOrEmpty(formContext.validate('submit'));
    }));
    return formContext;
}
Sys.Mvc.FormContext.prototype = {
    _onClickHandler: null,
    _onSubmitHandler: null,
    _submitButtonClicked: null,
    _validationSummaryElement: null,
    _validationSummaryULElement: null,
    _formElement: null,
    replaceValidationSummary: false,
    
    addError: function Sys_Mvc_FormContext$addError(message) {
        /// <param name="message" type="String">
        /// </param>
        this.addErrors([ message ]);
    },
    
    addErrors: function Sys_Mvc_FormContext$addErrors(messages) {
        /// <param name="messages" type="Array" elementType="String">
        /// </param>
        if (!Sys.Mvc._validationUtil.arrayIsNullOrEmpty(messages)) {
            Array.addRange(this._errors, messages);
            this._onErrorCountChanged();
        }
    },
    
    clearErrors: function Sys_Mvc_FormContext$clearErrors() {
        Array.clear(this._errors);
        this._onErrorCountChanged();
    },
    
    _displayError: function Sys_Mvc_FormContext$_displayError() {
        if (this._validationSummaryElement) {
            if (this._validationSummaryULElement) {
                Sys.Mvc._validationUtil.removeAllChildren(this._validationSummaryULElement);
                for (var i = 0; i < this._errors.length; i++) {
                    var liElement = document.createElement('li');
                    Sys.Mvc._validationUtil.setInnerText(liElement, this._errors[i]);
                    this._validationSummaryULElement.appendChild(liElement);
                }
            }
            Sys.UI.DomElement.removeCssClass(this._validationSummaryElement, Sys.Mvc.FormContext._validationSummaryValidCss);
            Sys.UI.DomElement.addCssClass(this._validationSummaryElement, Sys.Mvc.FormContext._validationSummaryErrorCss);
        }
    },
    
    _displaySuccess: function Sys_Mvc_FormContext$_displaySuccess() {
        var validationSummaryElement = this._validationSummaryElement;
        if (validationSummaryElement) {
            var validationSummaryULElement = this._validationSummaryULElement;
            if (validationSummaryULElement) {
                validationSummaryULElement.innerHTML = '';
            }
            Sys.UI.DomElement.removeCssClass(validationSummaryElement, Sys.Mvc.FormContext._validationSummaryErrorCss);
            Sys.UI.DomElement.addCssClass(validationSummaryElement, Sys.Mvc.FormContext._validationSummaryValidCss);
        }
    },
    
    enableDynamicValidation: function Sys_Mvc_FormContext$enableDynamicValidation() {
        Sys.UI.DomEvent.addHandler(this._formElement, 'click', this._onClickHandler);
        Sys.UI.DomEvent.addHandler(this._formElement, 'submit', this._onSubmitHandler);
    },
    
    _findSubmitButton: function Sys_Mvc_FormContext$_findSubmitButton(element) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <returns type="Object" domElement="true"></returns>
        if (element.disabled) {
            return null;
        }
        var tagName = element.tagName.toUpperCase();
        var inputElement = element;
        if (tagName === 'INPUT') {
            var type = inputElement.type;
            if (type === 'submit' || type === 'image') {
                return inputElement;
            }
        }
        else if ((tagName === 'BUTTON') && (inputElement.type === 'submit')) {
            return inputElement;
        }
        return null;
    },
    
    _form_OnClick: function Sys_Mvc_FormContext$_form_OnClick(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        this._submitButtonClicked = this._findSubmitButton(e.target);
    },
    
    _form_OnSubmit: function Sys_Mvc_FormContext$_form_OnSubmit(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        var form = e.target;
        var submitButton = this._submitButtonClicked;
        if (submitButton && submitButton.disableValidation) {
            return;
        }
        var errorMessages = this.validate('submit');
        if (!Sys.Mvc._validationUtil.arrayIsNullOrEmpty(errorMessages)) {
            e.preventDefault();
        }
    },
    
    _onErrorCountChanged: function Sys_Mvc_FormContext$_onErrorCountChanged() {
        if (!this._errors.length) {
            this._displaySuccess();
        }
        else {
            this._displayError();
        }
    },
    
    validate: function Sys_Mvc_FormContext$validate(eventName) {
        /// <param name="eventName" type="String">
        /// </param>
        /// <returns type="Array" elementType="String"></returns>
        var fields = this.fields;
        var errors = [];
        for (var i = 0; i < fields.length; i++) {
            var field = fields[i];
            var thisErrors = field.validate(eventName);
            if (thisErrors) {
                Array.addRange(errors, thisErrors);
            }
        }
        if (this.replaceValidationSummary) {
            this.clearErrors();
            this.addErrors(errors);
        }
        return errors;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.FieldContext

Sys.Mvc.FieldContext = function Sys_Mvc_FieldContext(formContext) {
    /// <param name="formContext" type="Sys.Mvc.FormContext">
    /// </param>
    /// <field name="_hasTextChangedTag" type="String" static="true">
    /// </field>
    /// <field name="_hasValidationFiredTag" type="String" static="true">
    /// </field>
    /// <field name="_inputElementErrorCss" type="String" static="true">
    /// </field>
    /// <field name="_inputElementValidCss" type="String" static="true">
    /// </field>
    /// <field name="_validationMessageErrorCss" type="String" static="true">
    /// </field>
    /// <field name="_validationMessageValidCss" type="String" static="true">
    /// </field>
    /// <field name="_onBlurHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_onChangeHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_onInputHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_onPropertyChangeHandler" type="Sys.UI.DomEventHandler">
    /// </field>
    /// <field name="_errors" type="Array">
    /// </field>
    /// <field name="defaultErrorMessage" type="String">
    /// </field>
    /// <field name="elements" type="Array" elementType="Object" elementDomElement="true">
    /// </field>
    /// <field name="formContext" type="Sys.Mvc.FormContext">
    /// </field>
    /// <field name="replaceValidationMessageContents" type="Boolean">
    /// </field>
    /// <field name="validationMessageElement" type="Object" domElement="true">
    /// </field>
    /// <field name="validations" type="Array" elementType="Validation">
    /// </field>
    this._errors = [];
    this.elements = new Array(0);
    this.validations = new Array(0);
    this.formContext = formContext;
    this._onBlurHandler = Function.createDelegate(this, this._element_OnBlur);
    this._onChangeHandler = Function.createDelegate(this, this._element_OnChange);
    this._onInputHandler = Function.createDelegate(this, this._element_OnInput);
    this._onPropertyChangeHandler = Function.createDelegate(this, this._element_OnPropertyChange);
}
Sys.Mvc.FieldContext.prototype = {
    _onBlurHandler: null,
    _onChangeHandler: null,
    _onInputHandler: null,
    _onPropertyChangeHandler: null,
    defaultErrorMessage: null,
    formContext: null,
    replaceValidationMessageContents: false,
    validationMessageElement: null,
    
    addError: function Sys_Mvc_FieldContext$addError(message) {
        /// <param name="message" type="String">
        /// </param>
        this.addErrors([ message ]);
    },
    
    addErrors: function Sys_Mvc_FieldContext$addErrors(messages) {
        /// <param name="messages" type="Array" elementType="String">
        /// </param>
        if (!Sys.Mvc._validationUtil.arrayIsNullOrEmpty(messages)) {
            Array.addRange(this._errors, messages);
            this._onErrorCountChanged();
        }
    },
    
    clearErrors: function Sys_Mvc_FieldContext$clearErrors() {
        Array.clear(this._errors);
        this._onErrorCountChanged();
    },
    
    _displayError: function Sys_Mvc_FieldContext$_displayError() {
        var validationMessageElement = this.validationMessageElement;
        if (validationMessageElement) {
            if (this.replaceValidationMessageContents) {
                Sys.Mvc._validationUtil.setInnerText(validationMessageElement, this._errors[0]);
            }
            Sys.UI.DomElement.removeCssClass(validationMessageElement, Sys.Mvc.FieldContext._validationMessageValidCss);
            Sys.UI.DomElement.addCssClass(validationMessageElement, Sys.Mvc.FieldContext._validationMessageErrorCss);
        }
        var elements = this.elements;
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            Sys.UI.DomElement.removeCssClass(element, Sys.Mvc.FieldContext._inputElementValidCss);
            Sys.UI.DomElement.addCssClass(element, Sys.Mvc.FieldContext._inputElementErrorCss);
        }
    },
    
    _displaySuccess: function Sys_Mvc_FieldContext$_displaySuccess() {
        var validationMessageElement = this.validationMessageElement;
        if (validationMessageElement) {
            if (this.replaceValidationMessageContents) {
                Sys.Mvc._validationUtil.setInnerText(validationMessageElement, '');
            }
            Sys.UI.DomElement.removeCssClass(validationMessageElement, Sys.Mvc.FieldContext._validationMessageErrorCss);
            Sys.UI.DomElement.addCssClass(validationMessageElement, Sys.Mvc.FieldContext._validationMessageValidCss);
        }
        var elements = this.elements;
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            Sys.UI.DomElement.removeCssClass(element, Sys.Mvc.FieldContext._inputElementErrorCss);
            Sys.UI.DomElement.addCssClass(element, Sys.Mvc.FieldContext._inputElementValidCss);
        }
    },
    
    _element_OnBlur: function Sys_Mvc_FieldContext$_element_OnBlur(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        if (e.target[Sys.Mvc.FieldContext._hasTextChangedTag] || e.target[Sys.Mvc.FieldContext._hasValidationFiredTag]) {
            this.validate('blur');
        }
    },
    
    _element_OnChange: function Sys_Mvc_FieldContext$_element_OnChange(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        e.target[Sys.Mvc.FieldContext._hasTextChangedTag] = true;
    },
    
    _element_OnInput: function Sys_Mvc_FieldContext$_element_OnInput(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        e.target[Sys.Mvc.FieldContext._hasTextChangedTag] = true;
        if (e.target[Sys.Mvc.FieldContext._hasValidationFiredTag]) {
            this.validate('input');
        }
    },
    
    _element_OnPropertyChange: function Sys_Mvc_FieldContext$_element_OnPropertyChange(e) {
        /// <param name="e" type="Sys.UI.DomEvent">
        /// </param>
        if (e.rawEvent.propertyName === 'value') {
            e.target[Sys.Mvc.FieldContext._hasTextChangedTag] = true;
            if (e.target[Sys.Mvc.FieldContext._hasValidationFiredTag]) {
                this.validate('input');
            }
        }
    },
    
    enableDynamicValidation: function Sys_Mvc_FieldContext$enableDynamicValidation() {
        var elements = this.elements;
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (Sys.Mvc._validationUtil.elementSupportsEvent(element, 'onpropertychange')) {
                var compatMode = document.documentMode;
                if (compatMode && compatMode >= 8) {
                    Sys.UI.DomEvent.addHandler(element, 'propertychange', this._onPropertyChangeHandler);
                }
            }
            else {
                Sys.UI.DomEvent.addHandler(element, 'input', this._onInputHandler);
            }
            Sys.UI.DomEvent.addHandler(element, 'change', this._onChangeHandler);
            Sys.UI.DomEvent.addHandler(element, 'blur', this._onBlurHandler);
        }
    },
    
    _getErrorString: function Sys_Mvc_FieldContext$_getErrorString(validatorReturnValue, fieldErrorMessage) {
        /// <param name="validatorReturnValue" type="Object">
        /// </param>
        /// <param name="fieldErrorMessage" type="String">
        /// </param>
        /// <returns type="String"></returns>
        var fallbackErrorMessage = fieldErrorMessage || this.defaultErrorMessage;
        if (Boolean.isInstanceOfType(validatorReturnValue)) {
            return (validatorReturnValue) ? null : fallbackErrorMessage;
        }
        if (String.isInstanceOfType(validatorReturnValue)) {
            return ((validatorReturnValue).length) ? validatorReturnValue : fallbackErrorMessage;
        }
        return null;
    },
    
    _getStringValue: function Sys_Mvc_FieldContext$_getStringValue() {
        /// <returns type="String"></returns>
        var elements = this.elements;
        return (elements.length > 0) ? elements[0].value : null;
    },
    
    _markValidationFired: function Sys_Mvc_FieldContext$_markValidationFired() {
        var elements = this.elements;
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            element[Sys.Mvc.FieldContext._hasValidationFiredTag] = true;
        }
    },
    
    _onErrorCountChanged: function Sys_Mvc_FieldContext$_onErrorCountChanged() {
        if (!this._errors.length) {
            this._displaySuccess();
        }
        else {
            this._displayError();
        }
    },
    
    validate: function Sys_Mvc_FieldContext$validate(eventName) {
        /// <param name="eventName" type="String">
        /// </param>
        /// <returns type="Array" elementType="String"></returns>
        var validations = this.validations;
        var errors = [];
        var value = this._getStringValue();
        for (var i = 0; i < validations.length; i++) {
            var validation = validations[i];
            var context = Sys.Mvc.$create_ValidationContext();
            context.eventName = eventName;
            context.fieldContext = this;
            context.validation = validation;
            var retVal = validation.validator(value, context);
            var errorMessage = this._getErrorString(retVal, validation.fieldErrorMessage);
            if (!Sys.Mvc._validationUtil.stringIsNullOrEmpty(errorMessage)) {
                Array.add(errors, errorMessage);
            }
        }
        this._markValidationFired();
        this.clearErrors();
        this.addErrors(errors);
        return errors;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.RangeValidator

Sys.Mvc.RangeValidator = function Sys_Mvc_RangeValidator(minimum, maximum) {
    /// <param name="minimum" type="Number">
    /// </param>
    /// <param name="maximum" type="Number">
    /// </param>
    /// <field name="_minimum" type="Number">
    /// </field>
    /// <field name="_maximum" type="Number">
    /// </field>
    this._minimum = minimum;
    this._maximum = maximum;
}
Sys.Mvc.RangeValidator.create = function Sys_Mvc_RangeValidator$create(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    var min = rule.ValidationParameters['minimum'];
    var max = rule.ValidationParameters['maximum'];
    return Function.createDelegate(new Sys.Mvc.RangeValidator(min, max), new Sys.Mvc.RangeValidator(min, max).validate);
}
Sys.Mvc.RangeValidator.prototype = {
    _minimum: null,
    _maximum: null,
    
    validate: function Sys_Mvc_RangeValidator$validate(value, context) {
        /// <param name="value" type="String">
        /// </param>
        /// <param name="context" type="Sys.Mvc.ValidationContext">
        /// </param>
        /// <returns type="Object"></returns>
        if (Sys.Mvc._validationUtil.stringIsNullOrEmpty(value)) {
            return true;
        }
        var n = Number.parseLocale(value);
        return (!isNaN(n) && this._minimum <= n && n <= this._maximum);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.RegularExpressionValidator

Sys.Mvc.RegularExpressionValidator = function Sys_Mvc_RegularExpressionValidator(pattern) {
    /// <param name="pattern" type="String">
    /// </param>
    /// <field name="_pattern" type="String">
    /// </field>
    this._pattern = pattern;
}
Sys.Mvc.RegularExpressionValidator.create = function Sys_Mvc_RegularExpressionValidator$create(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    var pattern = rule.ValidationParameters['pattern'];
    return Function.createDelegate(new Sys.Mvc.RegularExpressionValidator(pattern), new Sys.Mvc.RegularExpressionValidator(pattern).validate);
}
Sys.Mvc.RegularExpressionValidator.prototype = {
    _pattern: null,
    
    validate: function Sys_Mvc_RegularExpressionValidator$validate(value, context) {
        /// <param name="value" type="String">
        /// </param>
        /// <param name="context" type="Sys.Mvc.ValidationContext">
        /// </param>
        /// <returns type="Object"></returns>
        if (Sys.Mvc._validationUtil.stringIsNullOrEmpty(value)) {
            return true;
        }
        var regExp = new RegExp(this._pattern);
        var matches = regExp.exec(value);
        return (!Sys.Mvc._validationUtil.arrayIsNullOrEmpty(matches) && matches[0].length === value.length);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.RequiredValidator

Sys.Mvc.RequiredValidator = function Sys_Mvc_RequiredValidator() {
}
Sys.Mvc.RequiredValidator.create = function Sys_Mvc_RequiredValidator$create(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    return Function.createDelegate(new Sys.Mvc.RequiredValidator(), new Sys.Mvc.RequiredValidator().validate);
}
Sys.Mvc.RequiredValidator._isRadioInputElement = function Sys_Mvc_RequiredValidator$_isRadioInputElement(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <returns type="Boolean"></returns>
    if (element.tagName.toUpperCase() === 'INPUT') {
        var inputType = (element.type).toUpperCase();
        if (inputType === 'RADIO') {
            return true;
        }
    }
    return false;
}
Sys.Mvc.RequiredValidator._isSelectInputElement = function Sys_Mvc_RequiredValidator$_isSelectInputElement(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <returns type="Boolean"></returns>
    if (element.tagName.toUpperCase() === 'SELECT') {
        return true;
    }
    return false;
}
Sys.Mvc.RequiredValidator._isTextualInputElement = function Sys_Mvc_RequiredValidator$_isTextualInputElement(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <returns type="Boolean"></returns>
    if (element.tagName.toUpperCase() === 'INPUT') {
        var inputType = (element.type).toUpperCase();
        switch (inputType) {
            case 'TEXT':
            case 'PASSWORD':
            case 'FILE':
                return true;
        }
    }
    if (element.tagName.toUpperCase() === 'TEXTAREA') {
        return true;
    }
    return false;
}
Sys.Mvc.RequiredValidator._validateRadioInput = function Sys_Mvc_RequiredValidator$_validateRadioInput(elements) {
    /// <param name="elements" type="Array" elementType="Object" elementDomElement="true">
    /// </param>
    /// <returns type="Object"></returns>
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        if (element.checked) {
            return true;
        }
    }
    return false;
}
Sys.Mvc.RequiredValidator._validateSelectInput = function Sys_Mvc_RequiredValidator$_validateSelectInput(optionElements) {
    /// <param name="optionElements" type="DOMElementCollection">
    /// </param>
    /// <returns type="Object"></returns>
    for (var i = 0; i < optionElements.length; i++) {
        var element = optionElements[i];
        if (element.selected) {
            if (!Sys.Mvc._validationUtil.stringIsNullOrEmpty(element.value)) {
                return true;
            }
        }
    }
    return false;
}
Sys.Mvc.RequiredValidator._validateTextualInput = function Sys_Mvc_RequiredValidator$_validateTextualInput(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <returns type="Object"></returns>
    return (!Sys.Mvc._validationUtil.stringIsNullOrEmpty(element.value));
}
Sys.Mvc.RequiredValidator.prototype = {
    
    validate: function Sys_Mvc_RequiredValidator$validate(value, context) {
        /// <param name="value" type="String">
        /// </param>
        /// <param name="context" type="Sys.Mvc.ValidationContext">
        /// </param>
        /// <returns type="Object"></returns>
        var elements = context.fieldContext.elements;
        if (!elements.length) {
            return true;
        }
        var sampleElement = elements[0];
        if (Sys.Mvc.RequiredValidator._isTextualInputElement(sampleElement)) {
            return Sys.Mvc.RequiredValidator._validateTextualInput(sampleElement);
        }
        if (Sys.Mvc.RequiredValidator._isRadioInputElement(sampleElement)) {
            return Sys.Mvc.RequiredValidator._validateRadioInput(elements);
        }
        if (Sys.Mvc.RequiredValidator._isSelectInputElement(sampleElement)) {
            return Sys.Mvc.RequiredValidator._validateSelectInput((sampleElement).options);
        }
        return true;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.StringLengthValidator

Sys.Mvc.StringLengthValidator = function Sys_Mvc_StringLengthValidator(minLength, maxLength) {
    /// <param name="minLength" type="Number" integer="true">
    /// </param>
    /// <param name="maxLength" type="Number" integer="true">
    /// </param>
    /// <field name="_maxLength" type="Number" integer="true">
    /// </field>
    /// <field name="_minLength" type="Number" integer="true">
    /// </field>
    this._minLength = minLength;
    this._maxLength = maxLength;
}
Sys.Mvc.StringLengthValidator.create = function Sys_Mvc_StringLengthValidator$create(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    var minLength = rule.ValidationParameters['minimumLength'];
    var maxLength = rule.ValidationParameters['maximumLength'];
    return Function.createDelegate(new Sys.Mvc.StringLengthValidator(minLength, maxLength), new Sys.Mvc.StringLengthValidator(minLength, maxLength).validate);
}
Sys.Mvc.StringLengthValidator.prototype = {
    _maxLength: 0,
    _minLength: 0,
    
    validate: function Sys_Mvc_StringLengthValidator$validate(value, context) {
        /// <param name="value" type="String">
        /// </param>
        /// <param name="context" type="Sys.Mvc.ValidationContext">
        /// </param>
        /// <returns type="Object"></returns>
        if (Sys.Mvc._validationUtil.stringIsNullOrEmpty(value)) {
            return true;
        }
        return (this._minLength <= value.length && value.length <= this._maxLength);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc._validationUtil

Sys.Mvc._validationUtil = function Sys_Mvc__validationUtil() {
}
Sys.Mvc._validationUtil.arrayIsNullOrEmpty = function Sys_Mvc__validationUtil$arrayIsNullOrEmpty(array) {
    /// <param name="array" type="Array" elementType="Object">
    /// </param>
    /// <returns type="Boolean"></returns>
    return (!array || !array.length);
}
Sys.Mvc._validationUtil.stringIsNullOrEmpty = function Sys_Mvc__validationUtil$stringIsNullOrEmpty(value) {
    /// <param name="value" type="String">
    /// </param>
    /// <returns type="Boolean"></returns>
    return (!value || !value.length);
}
Sys.Mvc._validationUtil.elementSupportsEvent = function Sys_Mvc__validationUtil$elementSupportsEvent(element, eventAttributeName) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <param name="eventAttributeName" type="String">
    /// </param>
    /// <returns type="Boolean"></returns>
    return (eventAttributeName in element);
}
Sys.Mvc._validationUtil.removeAllChildren = function Sys_Mvc__validationUtil$removeAllChildren(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}
Sys.Mvc._validationUtil.setInnerText = function Sys_Mvc__validationUtil$setInnerText(element, innerText) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <param name="innerText" type="String">
    /// </param>
    var textNode = document.createTextNode(innerText);
    Sys.Mvc._validationUtil.removeAllChildren(element);
    element.appendChild(textNode);
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.ValidatorRegistry

Sys.Mvc.ValidatorRegistry = function Sys_Mvc_ValidatorRegistry() {
    /// <field name="validators" type="Object" static="true">
    /// </field>
}
Sys.Mvc.ValidatorRegistry.getValidator = function Sys_Mvc_ValidatorRegistry$getValidator(rule) {
    /// <param name="rule" type="Sys.Mvc.JsonValidationRule">
    /// </param>
    /// <returns type="Sys.Mvc.Validator"></returns>
    var creator = Sys.Mvc.ValidatorRegistry.validators[rule.ValidationType];
    return (creator) ? creator(rule) : null;
}
Sys.Mvc.ValidatorRegistry._getDefaultValidators = function Sys_Mvc_ValidatorRegistry$_getDefaultValidators() {
    /// <returns type="Object"></returns>
    return { required: Function.createDelegate(null, Sys.Mvc.RequiredValidator.create), stringLength: Function.createDelegate(null, Sys.Mvc.StringLengthValidator.create), regularExpression: Function.createDelegate(null, Sys.Mvc.RegularExpressionValidator.create), range: Function.createDelegate(null, Sys.Mvc.RangeValidator.create), number: Function.createDelegate(null, Sys.Mvc.NumberValidator.create) };
}


Sys.Mvc.NumberValidator.registerClass('Sys.Mvc.NumberValidator');
Sys.Mvc.FormContext.registerClass('Sys.Mvc.FormContext');
Sys.Mvc.FieldContext.registerClass('Sys.Mvc.FieldContext');
Sys.Mvc.RangeValidator.registerClass('Sys.Mvc.RangeValidator');
Sys.Mvc.RegularExpressionValidator.registerClass('Sys.Mvc.RegularExpressionValidator');
Sys.Mvc.RequiredValidator.registerClass('Sys.Mvc.RequiredValidator');
Sys.Mvc.StringLengthValidator.registerClass('Sys.Mvc.StringLengthValidator');
Sys.Mvc._validationUtil.registerClass('Sys.Mvc._validationUtil');
Sys.Mvc.ValidatorRegistry.registerClass('Sys.Mvc.ValidatorRegistry');
Sys.Mvc.FormContext._validationSummaryErrorCss = 'validation-summary-errors';
Sys.Mvc.FormContext._validationSummaryValidCss = 'validation-summary-valid';
Sys.Mvc.FormContext._formValidationTag = '__MVC_FormValidation';
Sys.Mvc.FieldContext._hasTextChangedTag = '__MVC_HasTextChanged';
Sys.Mvc.FieldContext._hasValidationFiredTag = '__MVC_HasValidationFired';
Sys.Mvc.FieldContext._inputElementErrorCss = 'input-validation-error';
Sys.Mvc.FieldContext._inputElementValidCss = 'input-validation-valid';
Sys.Mvc.FieldContext._validationMessageErrorCss = 'field-validation-error';
Sys.Mvc.FieldContext._validationMessageValidCss = 'field-validation-valid';
Sys.Mvc.ValidatorRegistry.validators = Sys.Mvc.ValidatorRegistry._getDefaultValidators();

// ---- Do not remove this footer ----
// Generated using Script# v0.5.0.0 (http://projects.nikhilk.net)
// -----------------------------------

// register validation
Sys.Application.add_load(function() {
  Sys.Application.remove_load(arguments.callee);
  Sys.Mvc.FormContext._Application_Load();
});

(function (b) {
    function s() { b.fn.ajaxSubmit.debug && window.console && window.console.log && window.console.log("[jquery.form] " + Array.prototype.join.call(arguments, "")) } b.fn.ajaxSubmit = function (a) {
        function e() {
            function o() {
                if (!z++) {
                    p.detachEvent ? p.detachEvent("onload", o) : p.removeEventListener("load", o, false); var q = true; try {
                        if (A) throw "timeout"; var n, m; m = p.contentWindow ? p.contentWindow.document : p.contentDocument ? p.contentDocument : p.document; var t = h.dataType == "xml" || m.XMLDocument || b.isXMLDoc(m); s("isXml=" +
t); if (!t && (m.body == null || m.body.innerHTML == "")) { if (--E) { z = 0; setTimeout(o, 100); return } s("Could not access iframe DOM after 50 tries."); return } i.responseText = m.body ? m.body.innerHTML : null; i.responseXML = m.XMLDocument ? m.XMLDocument : m; i.getResponseHeader = function (F) { return { "content-type": h.dataType}[F] }; if (h.dataType == "json" || h.dataType == "script") { var B = m.getElementsByTagName("textarea")[0]; if (B) i.responseText = B.value; else { var C = m.getElementsByTagName("pre")[0]; if (C) i.responseText = C.innerHTML } } else if (h.dataType ==
"xml" && !i.responseXML && i.responseText != null) i.responseXML = x(i.responseText); n = b.httpData(i, h.dataType)
                    } catch (G) { q = false; b.handleError(h, i, "error", G) } if (q) { h.success(n, "success"); u && b.event.trigger("ajaxSuccess", [i, h]) } u && b.event.trigger("ajaxComplete", [i, h]); u && ! --b.active && b.event.trigger("ajaxStop"); if (h.complete) h.complete(i, q ? "success" : "error"); setTimeout(function () { v.remove(); i.responseXML = null }, 100)
                } 
            } function x(q, n) {
                if (window.ActiveXObject) {
                    n = new ActiveXObject("Microsoft.XMLDOM"); n.async = "false";
                    n.loadXML(q)
                } else n = (new DOMParser).parseFromString(q, "text/xml"); return n && n.documentElement && n.documentElement.tagName != "parsererror" ? n : null
            } var j = g[0]; if (b(":input[name=submit]", j).length) alert('Error: Form elements must not be named "submit".'); else {
                var h = b.extend({}, b.ajaxSettings, a), r = b.extend(true, {}, b.extend(true, {}, b.ajaxSettings), h), y = "jqFormIO" + (new Date).getTime(), v = b('<iframe id="' + y + '" name="' + y + '" src="' + h.iframeSrc + '" />'), p = v[0]; v.css({ position: "absolute", top: "-1000px", left: "-1000px" });
                var i = { aborted: 0, responseText: null, responseXML: null, status: 0, statusText: "n/a", getAllResponseHeaders: function () { }, getResponseHeader: function () { }, setRequestHeader: function () { }, abort: function () { this.aborted = 1; v.attr("src", h.iframeSrc) } }, u = h.global; u && !b.active++ && b.event.trigger("ajaxStart"); u && b.event.trigger("ajaxSend", [i, h]); if (r.beforeSend && r.beforeSend(i, r) === false) r.global && b.active--; else if (!i.aborted) {
                    var z = 0, A = 0; if (r = j.clk) {
                        var D = r.name; if (D && !r.disabled) {
                            a.extraData = a.extraData || {}; a.extraData[D] =
r.value; if (r.type == "image") { a.extraData[name + ".x"] = j.clk_x; a.extraData[name + ".y"] = j.clk_y } 
                        } 
                    } setTimeout(function () {
                        var q = g.attr("target"), n = g.attr("action"); j.setAttribute("target", y); j.getAttribute("method") != "POST" && j.setAttribute("method", "POST"); j.getAttribute("action") != h.url && j.setAttribute("action", h.url); a.skipEncodingOverride || g.attr({ encoding: "multipart/form-data", enctype: "multipart/form-data" }); h.timeout && setTimeout(function () { A = true; o() }, h.timeout); var m = []; try {
                            if (a.extraData) for (var t in a.extraData) m.push(b('<input type="hidden" name="' +
t + '" value="' + a.extraData[t] + '" />').appendTo(j)[0]); v.appendTo("body"); p.attachEvent ? p.attachEvent("onload", o) : p.addEventListener("load", o, false); j.submit()
                        } finally { j.setAttribute("action", n); q ? j.setAttribute("target", q) : g.removeAttr("target"); b(m).remove() } 
                    }, 10); var E = 50
                } 
            } 
        } if (!this.length) { s("ajaxSubmit: skipping submit process - no element selected"); return this } if (typeof a == "function") a = { success: a }; var d = b.trim(this.attr("action")); if (d) d = (d.match(/^([^#]+)/) || [])[1]; d = d || window.location.href ||
""; a = b.extend({ url: d, type: this.attr("method") || "GET", iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank" }, a || {}); d = {}; this.trigger("form-pre-serialize", [this, a, d]); if (d.veto) { s("ajaxSubmit: submit vetoed via form-pre-serialize trigger"); return this } if (a.beforeSerialize && a.beforeSerialize(this, a) === false) { s("ajaxSubmit: submit aborted via beforeSerialize callback"); return this } var f = this.formToArray(a.semantic); if (a.data) {
            a.extraData = a.data; for (var c in a.data) if (a.data[c] instanceof
Array) for (var l in a.data[c]) f.push({ name: c, value: a.data[c][l] }); else f.push({ name: c, value: a.data[c] })
        } if (a.beforeSubmit && a.beforeSubmit(f, this, a) === false) { s("ajaxSubmit: submit aborted via beforeSubmit callback"); return this } this.trigger("form-submit-validate", [f, this, a, d]); if (d.veto) { s("ajaxSubmit: submit vetoed via form-submit-validate trigger"); return this } c = b.param(f); if (a.type.toUpperCase() == "GET") { a.url += (a.url.indexOf("?") >= 0 ? "&" : "?") + c; a.data = null } else a.data = c; var g = this, k = []; a.resetForm &&
k.push(function () { g.resetForm() }); a.clearForm && k.push(function () { g.clearForm() }); if (!a.dataType && a.target) { var w = a.success || function () { }; k.push(function (o) { b(a.target).html(o).each(w, arguments) }) } else a.success && k.push(a.success); a.success = function (o, x) { for (var j = 0, h = k.length; j < h; j++) k[j].apply(a, [o, x, g]) }; c = b("input:file", this).fieldValue(); l = false; for (d = 0; d < c.length; d++) if (c[d]) l = true; if (c.length && a.iframe !== false || a.iframe || l || 0) a.closeKeepAlive ? b.get(a.closeKeepAlive, e) : e(); else b.ajax(a); this.trigger("form-submit-notify",
[this, a]); return this
    }; b.fn.ajaxForm = function (a) {
        return this.ajaxFormUnbind().bind("submit.form-plugin", function () { b(this).ajaxSubmit(a); return false }).bind("click.form-plugin", function (e) {
            var d = e.target, f = b(d); if (!f.is(":submit,input:image")) { d = f.closest(":submit"); if (d.length == 0) return; d = d[0] } var c = this; c.clk = d; if (d.type == "image") if (e.offsetX != undefined) { c.clk_x = e.offsetX; c.clk_y = e.offsetY } else if (typeof b.fn.offset == "function") { f = f.offset(); c.clk_x = e.pageX - f.left; c.clk_y = e.pageY - f.top } else {
                c.clk_x =
e.pageX - d.offsetLeft; c.clk_y = e.pageY - d.offsetTop
            } setTimeout(function () { c.clk = c.clk_x = c.clk_y = null }, 100)
        })
    }; b.fn.ajaxFormUnbind = function () { return this.unbind("submit.form-plugin click.form-plugin") }; b.fn.formToArray = function (a) {
        var e = []; if (this.length == 0) return e; var d = this[0], f = a ? d.getElementsByTagName("*") : d.elements; if (!f) return e; for (var c = 0, l = f.length; c < l; c++) {
            var g = f[c], k = g.name; if (k) if (a && d.clk && g.type == "image") {
                if (!g.disabled && d.clk == g) {
                    e.push({ name: k, value: b(g).val() }); e.push({ name: k + ".x",
                        value: d.clk_x
                    }, { name: k + ".y", value: d.clk_y })
                } 
            } else if ((g = b.fieldValue(g, true)) && g.constructor == Array) for (var w = 0, o = g.length; w < o; w++) e.push({ name: k, value: g[w] }); else g !== null && typeof g != "undefined" && e.push({ name: k, value: g })
        } if (!a && d.clk) { a = b(d.clk); f = a[0]; if ((k = f.name) && !f.disabled && f.type == "image") { e.push({ name: k, value: a.val() }); e.push({ name: k + ".x", value: d.clk_x }, { name: k + ".y", value: d.clk_y }) } } return e
    }; b.fn.formSerialize = function (a) { return b.param(this.formToArray(a)) }; b.fn.fieldSerialize = function (a) {
        var e =
[]; this.each(function () { var d = this.name; if (d) { var f = b.fieldValue(this, a); if (f && f.constructor == Array) for (var c = 0, l = f.length; c < l; c++) e.push({ name: d, value: f[c] }); else f !== null && typeof f != "undefined" && e.push({ name: this.name, value: f }) } }); return b.param(e)
    }; b.fn.fieldValue = function (a) { for (var e = [], d = 0, f = this.length; d < f; d++) { var c = b.fieldValue(this[d], a); c === null || typeof c == "undefined" || c.constructor == Array && !c.length || (c.constructor == Array ? b.merge(e, c) : e.push(c)) } return e }; b.fieldValue = function (a, e) {
        var d =
a.name, f = a.type, c = a.tagName.toLowerCase(); if (typeof e == "undefined") e = true; if (e && (!d || a.disabled || f == "reset" || f == "button" || (f == "checkbox" || f == "radio") && !a.checked || (f == "submit" || f == "image") && a.form && a.form.clk != a || c == "select" && a.selectedIndex == -1)) return null; if (c == "select") {
            c = a.selectedIndex; if (c < 0) return null; e = []; a = a.options; d = (f = f == "select-one") ? c + 1 : a.length; for (c = f ? c : 0; c < d; c++) {
                var l = a[c]; if (l.selected) {
                    var g = l.value; g || (g = l.attributes && l.attributes.value && !l.attributes.value.specified ? l.text :
l.value); if (f) return g; e.push(g)
                } 
            } return e
        } return a.value
    }; b.fn.clearForm = function () { return this.each(function () { b("input,select,textarea", this).clearFields() }) }; b.fn.clearFields = b.fn.clearInputs = function () { return this.each(function () { var a = this.type, e = this.tagName.toLowerCase(); if (a == "text" || a == "password" || e == "textarea") this.value = ""; else if (a == "checkbox" || a == "radio") this.checked = false; else if (e == "select") this.selectedIndex = -1 }) }; b.fn.resetForm = function () {
        return this.each(function () {
            if (typeof this.reset ==
"function" || typeof this.reset == "object" && !this.reset.nodeType) this.reset()
        })
    }; b.fn.enable = function (a) { if (a == undefined) a = true; return this.each(function () { this.disabled = !a }) }; b.fn.selected = function (a) { if (a == undefined) a = true; return this.each(function () { var e = this.type; if (e == "checkbox" || e == "radio") this.checked = a; else if (this.tagName.toLowerCase() == "option") { e = b(this).parent("select"); a && e[0] && e[0].type == "select-one" && e.find("option").selected(false); this.selected = a } }) } 
})(jQuery);
/*!
 * jQuery blockUI plugin
 * Version 2.28 (02-DEC-2009)
 * @requires jQuery v1.2.3 or later
 *
 * Examples at: http://malsup.com/jquery/block/
 * Copyright (c) 2007-2008 M. Alsup
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 * Thanks to Amir-Hossein Sobhi for some excellent contributions!
 */

;(function($) {

if (/1\.(0|1|2)\.(0|1|2)/.test($.fn.jquery) || /^1.1/.test($.fn.jquery)) {
	alert('blockUI requires jQuery v1.2.3 or later!  You are using v' + $.fn.jquery);
	return;
}

$.fn._fadeIn = $.fn.fadeIn;

// this bit is to ensure we don't call setExpression when we shouldn't (with extra muscle to handle
// retarded userAgent strings on Vista)
var mode = document.documentMode || 0;
var setExpr = $.browser.msie && (($.browser.version < 8 && !mode) || mode < 8);
var ie6 = $.browser.msie && /MSIE 6.0/.test(navigator.userAgent) && !mode;

// global $ methods for blocking/unblocking the entire page
$.blockUI   = function(opts) { install(window, opts); };
$.unblockUI = function(opts) { remove(window, opts); };

// convenience method for quick growl-like notifications  (http://www.google.com/search?q=growl)
$.growlUI = function(title, message, timeout, onClose) {
	var $m = $('<div class="growlUI"></div>');
	if (title) $m.append('<h1>'+title+'</h1>');
	if (message) $m.append('<h2>'+message+'</h2>');
	if (timeout == undefined) timeout = 3000;
	$.blockUI({
		message: $m, fadeIn: 700, fadeOut: 1000, centerY: false,
		timeout: timeout, showOverlay: false,
		onUnblock: onClose, 
		css: $.blockUI.defaults.growlCSS
	});
};

// plugin method for blocking element content
$.fn.block = function(opts) {
	return this.unblock({ fadeOut: 0 }).each(function() {
		if ($.css(this,'position') == 'static')
			this.style.position = 'relative';
		if ($.browser.msie)
			this.style.zoom = 1; // force 'hasLayout'
		install(this, opts);
	});
};

// plugin method for unblocking element content
$.fn.unblock = function(opts) {
	return this.each(function() {
		remove(this, opts);
	});
};

$.blockUI.version = 2.28; // 2nd generation blocking at no extra cost!

// override these in your code to change the default behavior and style
$.blockUI.defaults = {
	// message displayed when blocking (use null for no message)
	message:  '<h1>Please wait...</h1>',

	title: null,	  // title string; only used when theme == true
	draggable: true,  // only used when theme == true (requires jquery-ui.js to be loaded)
	
	theme: false, // set to true to use with jQuery UI themes
	
	// styles for the message when blocking; if you wish to disable
	// these and use an external stylesheet then do this in your code:
	// $.blockUI.defaults.css = {};
	css: {
		padding:	0,
		margin:		0,
		width:		'30%',
		top:		'40%',
		left:		'35%',
		textAlign:	'center',
		color:		'#000',
		border:		'3px solid #aaa',
		backgroundColor:'#fff',
		cursor:		'wait'
	},
	
	// minimal style set used when themes are used
	themedCSS: {
		width:	'30%',
		top:	'40%',
		left:	'35%'
	},

	// styles for the overlay
	overlayCSS:  {
		backgroundColor: '#000',
		opacity:	  	 0.6,
		cursor:		  	 'wait'
	},

	// styles applied when using $.growlUI
	growlCSS: {
		width:  	'350px',
		top:		'10px',
		left:   	'',
		right:  	'10px',
		border: 	'none',
		padding:	'5px',
		opacity:	0.6,
		cursor: 	'default',
		color:		'#fff',
		backgroundColor: '#000',
		'-webkit-border-radius': '10px',
		'-moz-border-radius':	 '10px'
	},
	
	// IE issues: 'about:blank' fails on HTTPS and javascript:false is s-l-o-w
	// (hat tip to Jorge H. N. de Vasconcelos)
	iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',

	// force usage of iframe in non-IE browsers (handy for blocking applets)
	forceIframe: false,

	// z-index for the blocking overlay
	baseZ: 1000,

	// set these to true to have the message automatically centered
	centerX: true, // <-- only effects element blocking (page block controlled via css above)
	centerY: true,

	// allow body element to be stetched in ie6; this makes blocking look better
	// on "short" pages.  disable if you wish to prevent changes to the body height
	allowBodyStretch: true,

	// enable if you want key and mouse events to be disabled for content that is blocked
	bindEvents: true,

	// be default blockUI will supress tab navigation from leaving blocking content
	// (if bindEvents is true)
	constrainTabKey: true,

	// fadeIn time in millis; set to 0 to disable fadeIn on block
	fadeIn:  200,

	// fadeOut time in millis; set to 0 to disable fadeOut on unblock
	fadeOut:  400,

	// time in millis to wait before auto-unblocking; set to 0 to disable auto-unblock
	timeout: 0,

	// disable if you don't want to show the overlay
	showOverlay: true,

	// if true, focus will be placed in the first available input field when
	// page blocking
	focusInput: true,

	// suppresses the use of overlay styles on FF/Linux (due to performance issues with opacity)
	applyPlatformOpacityRules: true,

	// callback method invoked when unblocking has completed; the callback is
	// passed the element that has been unblocked (which is the window object for page
	// blocks) and the options that were passed to the unblock call:
	//	 onUnblock(element, options)
	onUnblock: null,

	// don't ask; if you really must know: http://groups.google.com/group/jquery-en/browse_thread/thread/36640a8730503595/2f6a79a77a78e493#2f6a79a77a78e493
	quirksmodeOffsetHack: 4
};

// private data and functions follow...

var pageBlock = null;
var pageBlockEls = [];

function install(el, opts) {
	var full = (el == window);
	var msg = opts && opts.message !== undefined ? opts.message : undefined;
	opts = $.extend({}, $.blockUI.defaults, opts || {});
	opts.overlayCSS = $.extend({}, $.blockUI.defaults.overlayCSS, opts.overlayCSS || {});
	var css = $.extend({}, $.blockUI.defaults.css, opts.css || {});
	var themedCSS = $.extend({}, $.blockUI.defaults.themedCSS, opts.themedCSS || {});
	msg = msg === undefined ? opts.message : msg;

	// remove the current block (if there is one)
	if (full && pageBlock)
		remove(window, {fadeOut:0});

	// if an existing element is being used as the blocking content then we capture
	// its current place in the DOM (and current display style) so we can restore
	// it when we unblock
	if (msg && typeof msg != 'string' && (msg.parentNode || msg.jquery)) {
		var node = msg.jquery ? msg[0] : msg;
		var data = {};
		$(el).data('blockUI.history', data);
		data.el = node;
		data.parent = node.parentNode;
		data.display = node.style.display;
		data.position = node.style.position;
		if (data.parent)
			data.parent.removeChild(node);
	}

	var z = opts.baseZ;

	// blockUI uses 3 layers for blocking, for simplicity they are all used on every platform;
	// layer1 is the iframe layer which is used to supress bleed through of underlying content
	// layer2 is the overlay layer which has opacity and a wait cursor (by default)
	// layer3 is the message content that is displayed while blocking

	var lyr1 = ($.browser.msie || opts.forceIframe) 
		? $('<iframe class="blockUI" style="z-index:'+ (z++) +';display:none;border:none;margin:0;padding:0;position:absolute;width:100%;height:100%;top:0;left:0" src="'+opts.iframeSrc+'"></iframe>')
		: $('<div class="blockUI" style="display:none"></div>');
	var lyr2 = $('<div class="blockUI blockOverlay" style="z-index:'+ (z++) +';display:none;border:none;margin:0;padding:0;width:100%;height:100%;top:0;left:0"></div>');
	
	var lyr3;
	if (opts.theme && full) {
		var s = '<div class="blockUI blockMsg blockPage ui-dialog ui-widget ui-corner-all" style="z-index:'+z+';display:none;position:fixed">' +
					'<div class="ui-widget-header ui-dialog-titlebar blockTitle">'+(opts.title || '&nbsp;')+'</div>' +
					'<div class="ui-widget-content ui-dialog-content"></div>' +
				'</div>';
		lyr3 = $(s);
	}
	else {
		lyr3 = full ? $('<div class="blockUI blockMsg blockPage" style="z-index:'+z+';display:none;position:fixed"></div>')
					: $('<div class="blockUI blockMsg blockElement" style="z-index:'+z+';display:none;position:absolute"></div>');
	}						   

	// if we have a message, style it
	if (msg) {
		if (opts.theme) {
			lyr3.css(themedCSS);
			lyr3.addClass('ui-widget-content');
		}
		else 
			lyr3.css(css);
	}

	// style the overlay
	if (!opts.applyPlatformOpacityRules || !($.browser.mozilla && /Linux/.test(navigator.platform)))
		lyr2.css(opts.overlayCSS);
	lyr2.css('position', full ? 'fixed' : 'absolute');

	// make iframe layer transparent in IE
	if ($.browser.msie || opts.forceIframe)
		lyr1.css('opacity',0.0);

	$([lyr1[0],lyr2[0],lyr3[0]]).appendTo(full ? 'body' : el);
	
	if (opts.theme && opts.draggable && $.fn.draggable) {
		lyr3.draggable({
			handle: '.ui-dialog-titlebar',
			cancel: 'li'
		});
	}

	// ie7 must use absolute positioning in quirks mode and to account for activex issues (when scrolling)
	var expr = setExpr && (!$.boxModel || $('object,embed', full ? null : el).length > 0);
	if (ie6 || expr) {
		// give body 100% height
		if (full && opts.allowBodyStretch && $.boxModel)
			$('html,body').css('height','100%');

		// fix ie6 issue when blocked element has a border width
		if ((ie6 || !$.boxModel) && !full) {
			var t = sz(el,'borderTopWidth'), l = sz(el,'borderLeftWidth');
			var fixT = t ? '(0 - '+t+')' : 0;
			var fixL = l ? '(0 - '+l+')' : 0;
		}

		// simulate fixed position
		$.each([lyr1,lyr2,lyr3], function(i,o) {
			var s = o[0].style;
			s.position = 'absolute';
			if (i < 2) {
				full ? s.setExpression('height','Math.max(document.body.scrollHeight, document.body.offsetHeight) - (jQuery.boxModel?0:'+opts.quirksmodeOffsetHack+') + "px"')
					 : s.setExpression('height','this.parentNode.offsetHeight + "px"');
				full ? s.setExpression('width','jQuery.boxModel && document.documentElement.clientWidth || document.body.clientWidth + "px"')
					 : s.setExpression('width','this.parentNode.offsetWidth + "px"');
				if (fixL) s.setExpression('left', fixL);
				if (fixT) s.setExpression('top', fixT);
			}
			else if (opts.centerY) {
				if (full) s.setExpression('top','(document.documentElement.clientHeight || document.body.clientHeight) / 2 - (this.offsetHeight / 2) + (blah = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + "px"');
				s.marginTop = 0;
			}
			else if (!opts.centerY && full) {
				var top = (opts.css && opts.css.top) ? parseInt(opts.css.top) : 0;
				var expression = '((document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + '+top+') + "px"';
				s.setExpression('top',expression);
			}
		});
	}

	// show the message
	if (msg) {
		if (opts.theme)
			lyr3.find('.ui-widget-content').append(msg);
		else
			lyr3.append(msg);
		if (msg.jquery || msg.nodeType)
			$(msg).show();
	}

	if (($.browser.msie || opts.forceIframe) && opts.showOverlay)
		lyr1.show(); // opacity is zero
	if (opts.fadeIn) {
		if (opts.showOverlay)
			lyr2._fadeIn(opts.fadeIn);
		if (msg)
			lyr3.fadeIn(opts.fadeIn);
	}
	else {
		if (opts.showOverlay)
			lyr2.show();
		if (msg)
			lyr3.show();
	}

	// bind key and mouse events
	bind(1, el, opts);

	if (full) {
		pageBlock = lyr3[0];
		pageBlockEls = $(':input:enabled:visible',pageBlock);
		if (opts.focusInput)
			setTimeout(focus, 20);
	}
	else
		center(lyr3[0], opts.centerX, opts.centerY);

	if (opts.timeout) {
		// auto-unblock
		var to = setTimeout(function() {
			full ? $.unblockUI(opts) : $(el).unblock(opts);
		}, opts.timeout);
		$(el).data('blockUI.timeout', to);
	}
};

// remove the block
function remove(el, opts) {
	var full = (el == window);
	var $el = $(el);
	var data = $el.data('blockUI.history');
	var to = $el.data('blockUI.timeout');
	if (to) {
		clearTimeout(to);
		$el.removeData('blockUI.timeout');
	}
	opts = $.extend({}, $.blockUI.defaults, opts || {});
	bind(0, el, opts); // unbind events
	
	var els;
	if (full) // crazy selector to handle odd field errors in ie6/7
		els = $('body').children().filter('.blockUI').add('body > .blockUI');
	else
		els = $('.blockUI', el);

	if (full)
		pageBlock = pageBlockEls = null;

	if (opts.fadeOut) {
		els.fadeOut(opts.fadeOut);
		setTimeout(function() { reset(els,data,opts,el); }, opts.fadeOut);
	}
	else
		reset(els, data, opts, el);
};

// move blocking element back into the DOM where it started
function reset(els,data,opts,el) {
	els.each(function(i,o) {
		// remove via DOM calls so we don't lose event handlers
		if (this.parentNode)
			this.parentNode.removeChild(this);
	});

	if (data && data.el) {
		data.el.style.display = data.display;
		data.el.style.position = data.position;
		if (data.parent)
			data.parent.appendChild(data.el);
		$(el).removeData('blockUI.history');
	}

	if (typeof opts.onUnblock == 'function')
		opts.onUnblock(el,opts);
};

// bind/unbind the handler
function bind(b, el, opts) {
	var full = el == window, $el = $(el);

	// don't bother unbinding if there is nothing to unbind
	if (!b && (full && !pageBlock || !full && !$el.data('blockUI.isBlocked')))
		return;
	if (!full)
		$el.data('blockUI.isBlocked', b);

	// don't bind events when overlay is not in use or if bindEvents is false
	if (!opts.bindEvents || (b && !opts.showOverlay)) 
		return;

	// bind anchors and inputs for mouse and key events
	var events = 'mousedown mouseup keydown keypress';
	b ? $(document).bind(events, opts, handler) : $(document).unbind(events, handler);

// former impl...
//	   var $e = $('a,:input');
//	   b ? $e.bind(events, opts, handler) : $e.unbind(events, handler);
};

// event handler to suppress keyboard/mouse events when blocking
function handler(e) {
	// allow tab navigation (conditionally)
	if (e.keyCode && e.keyCode == 9) {
		if (pageBlock && e.data.constrainTabKey) {
			var els = pageBlockEls;
			var fwd = !e.shiftKey && e.target == els[els.length-1];
			var back = e.shiftKey && e.target == els[0];
			if (fwd || back) {
				setTimeout(function(){focus(back)},10);
				return false;
			}
		}
	}
	// allow events within the message content
	if ($(e.target).parents('div.blockMsg').length > 0)
		return true;

	// allow events for content that is not being blocked
	return $(e.target).parents().children().filter('div.blockUI').length == 0;
};

function focus(back) {
	if (!pageBlockEls)
		return;
	var e = pageBlockEls[back===true ? pageBlockEls.length-1 : 0];
	if (e)
		e.focus();
};

function center(el, x, y) {
	var p = el.parentNode, s = el.style;
	var l = ((p.offsetWidth - el.offsetWidth)/2) - sz(p,'borderLeftWidth');
	var t = ((p.offsetHeight - el.offsetHeight)/2) - sz(p,'borderTopWidth');
	if (x) s.left = l > 0 ? (l+'px') : '0';
	if (y) s.top  = t > 0 ? (t+'px') : '0';
};

function sz(el, p) {
	return parseInt($.css(el,p))||0;
};

})(jQuery);

(function($){$.gritter={};$.gritter.options={fade_in_speed:'medium',fade_out_speed:1000,time:6000}
$.gritter.add=function(params){try{return Gritter.add(params||{});}catch(e){var err='Gritter Error: '+e;(typeof(console)!='undefined'&&console.error)?console.error(err,params):alert(err);}}
$.gritter.remove=function(id,params){Gritter.removeSpecific(id,params||{});}
$.gritter.removeAll=function(params){Gritter.stop(params||{});}
var Gritter={fade_in_speed:'',fade_out_speed:'',time:'',_custom_timer:0,_item_count:0,_is_setup:0,_tpl_close:'<div class="gritter-close"></div>',_tpl_item:'<div id="gritter-item-[[number]]" class="gritter-item-wrapper [[item_class]]" style="display:none"><div class="gritter-top"></div><div class="gritter-item">[[image]]<div class="[[class_name]]"><span class="gritter-title">[[username]]</span><p>[[text]]</p></div><div style="clear:both"></div></div><div class="gritter-bottom"></div></div>',_tpl_wrap:'<div id="gritter-notice-wrapper"></div>',add:function(params){if(!params.title||!params.text){throw'You need to fill out the first 2 params: "title" and "text"';}
if(!this._is_setup){this._runSetup();}
var user=params.title,text=params.text,image=params.image||'',sticky=params.sticky||false,item_class=params.class_name||'',time_alive=params.time||'';this._verifyWrapper();this._item_count++;var number=this._item_count,tmp=this._tpl_item;$(['before_open','after_open','before_close','after_close']).each(function(i,val){Gritter['_'+val+'_'+number]=($.isFunction(params[val]))?params[val]:function(){}});this._custom_timer=0;if(time_alive){this._custom_timer=time_alive;}
var image_str=(image!='')?'<img src="'+image+'" class="gritter-image" />':'',class_name=(image!='')?'gritter-with-image':'gritter-without-image';tmp=this._str_replace(['[[username]]','[[text]]','[[image]]','[[number]]','[[class_name]]','[[item_class]]'],[user,text,image_str,this._item_count,class_name,item_class],tmp);this['_before_open_'+number]();$('#gritter-notice-wrapper').append(tmp);var item=$('#gritter-item-'+this._item_count);item.fadeIn(this.fade_in_speed,function(){Gritter['_after_open_'+number]($(this));});if(!sticky){this._setFadeTimer(item,number);}
$(item).bind('mouseenter mouseleave',function(event){if(event.type=='mouseenter'){if(!sticky){Gritter._restoreItemIfFading($(this),number);}}
else{if(!sticky){Gritter._setFadeTimer($(this),number);}}
Gritter._hoverState($(this),event.type);});return number;},_countRemoveWrapper:function(unique_id,e){e.remove();this['_after_close_'+unique_id](e);if($('.gritter-item-wrapper').length==0){$('#gritter-notice-wrapper').remove();}},_fade:function(e,unique_id,params,unbind_events){var params=params||{},fade=(typeof(params.fade)!='undefined')?params.fade:true;fade_out_speed=params.speed||this.fade_out_speed;this['_before_close_'+unique_id](e);if(unbind_events){e.unbind('mouseenter mouseleave');}
if(fade){e.animate({opacity:0},fade_out_speed,function(){e.animate({height:0},300,function(){Gritter._countRemoveWrapper(unique_id,e);})})}
else{this._countRemoveWrapper(unique_id,e);}},_hoverState:function(e,type){if(type=='mouseenter'){e.addClass('hover');var find_img=e.find('img');(find_img.length)?find_img.before(this._tpl_close):e.find('span').before(this._tpl_close);e.find('.gritter-close').click(function(){var unique_id=e.attr('id').split('-')[2];Gritter.removeSpecific(unique_id,{},e,true);});}
else{e.removeClass('hover');e.find('.gritter-close').remove();}},removeSpecific:function(unique_id,params,e,unbind_events){if(!e){var e=$('#gritter-item-'+unique_id);}
this._fade(e,unique_id,params||{},unbind_events);},_restoreItemIfFading:function(e,unique_id){clearTimeout(this['_int_id_'+unique_id]);e.stop().css({opacity:''});},_runSetup:function(){for(opt in $.gritter.options){this[opt]=$.gritter.options[opt];}
this._is_setup=1;},_setFadeTimer:function(e,unique_id){var timer_str=(this._custom_timer)?this._custom_timer:this.time;this['_int_id_'+unique_id]=setTimeout(function(){Gritter._fade(e,unique_id);},timer_str);},stop:function(params){var before_close=($.isFunction(params.before_close))?params.before_close:function(){};var after_close=($.isFunction(params.after_close))?params.after_close:function(){};var wrap=$('#gritter-notice-wrapper');before_close(wrap);wrap.fadeOut(function(){$(this).remove();after_close();});},_str_replace:function(search,replace,subject,count){var i=0,j=0,temp='',repl='',sl=0,fl=0,f=[].concat(search),r=[].concat(replace),s=subject,ra=r instanceof Array,sa=s instanceof Array;s=[].concat(s);if(count){this.window[count]=0;}
for(i=0,sl=s.length;i<sl;i++){if(s[i]===''){continue;}
for(j=0,fl=f.length;j<fl;j++){temp=s[i]+'';repl=ra?(r[j]!==undefined?r[j]:''):r[0];s[i]=(temp).split(f[j]).join(repl);if(count&&s[i]!==temp){this.window[count]+=(temp.length-s[i].length)/f[j].length;}}}
return sa?s:s[0];},_verifyWrapper:function(){if($('#gritter-notice-wrapper').length==0){$('body').append(this._tpl_wrap);}}}})(jQuery);
/** @license

 SoundManager 2: Javascript Sound for the Web
 --------------------------------------------
 http://schillmania.com/projects/soundmanager2/

 Copyright (c) 2007, Scott Schiller. All rights reserved.
 Code provided under the BSD License:
 http://schillmania.com/projects/soundmanager2/license.txt

 V2.97a.20101010
*/
(function(T){function oa(Ga,Ha){function pa(){if(b.debugURLParam.test(U))b.debugMode=true}this.flashVersion=8;this.debugFlash=this.debugMode=false;this.useConsole=true;this.waitForWindowLoad=this.consoleOnly=false;this.nullURL="about:blank";this.allowPolling=true;this.useFastPolling=false;this.useMovieStar=true;this.bgColor="#ffffff";this.useHighPerformance=false;this.flashLoadTimeout=1E3;this.wmode=null;this.allowScriptAccess="always";this.useHTML5Audio=this.useFlashBlock=false;this.html5Test=/^probably$/i;
this.ondebuglog=false;this.audioFormats={mp3:{type:['audio/mpeg; codecs="mp3"',"audio/mpeg","audio/mp3","audio/MPA","audio/mpa-robust"],required:true},mp4:{related:["aac","m4a"],type:['audio/mp4; codecs="mp4a.40.2"',"audio/aac","audio/x-m4a","audio/MP4A-LATM","audio/mpeg4-generic"],required:true},ogg:{type:["audio/ogg; codecs=vorbis"],required:false},wav:{type:['audio/wav; codecs="1"',"audio/wav","audio/wave","audio/x-wav"],required:false}};this.defaultOptions={autoLoad:false,stream:true,autoPlay:false,
loops:1,onid3:null,onload:null,whileloading:null,onplay:null,onpause:null,onresume:null,whileplaying:null,onstop:null,onfailure:null,onfinish:null,onbeforefinish:null,onbeforefinishtime:5E3,onbeforefinishcomplete:null,onjustbeforefinish:null,onjustbeforefinishtime:200,multiShot:true,multiShotEvents:false,position:null,pan:0,type:null,usePolicyFile:false,volume:100};this.flash9Options={isMovieStar:null,usePeakData:false,useWaveformData:false,useEQData:false,onbufferchange:null,ondataerror:null,onstats:null};
this.movieStarOptions={bufferTime:3,serverURL:null,onconnect:null,bufferTimes:null,duration:null};this.version=null;this.versionNumber="V2.97a.20101010";this.movieURL=null;this.url=Ga||null;this.altURL=null;this.enabled=this.swfLoaded=false;this.o=null;this.movieID="sm2-container";this.id=Ha||"sm2movie";this.swfCSS={swfBox:"sm2-object-box",swfDefault:"movieContainer",swfError:"swf_error",swfTimedout:"swf_timedout",swfUnblocked:"swf_unblocked",sm2Debug:"sm2_debug",highPerf:"high_performance",flashDebug:"flash_debug"};
this.oMC=null;this.sounds={};this.soundIDs=[];this.muted=false;this.debugID="soundmanager-debug";this.debugURLParam=/([#?&])debug=1/i;this.didFlashBlock=this.specialWmodeCase=false;this.filePattern=null;this.filePatterns={flash8:/\.mp3(\?.*)?$/i,flash9:/\.mp3(\?.*)?$/i};this.baseMimeTypes=/^\s*audio\/(?:x-)?(?:mp(?:eg|3))\s*(?:$|;)/i;this.netStreamMimeTypes=/^\s*audio\/(?:x-)?(?:mp(?:eg|3))\s*(?:$|;)/i;this.netStreamTypes=["aac","flv","mov","mp4","m4v","f4v","m4a","mp4v","3gp","3g2"];this.netStreamPattern=
RegExp("\\.("+this.netStreamTypes.join("|")+")(\\?.*)?$","i");this.mimePattern=this.baseMimeTypes;this.features={buffering:false,peakData:false,waveformData:false,eqData:false,movieStar:false};this.sandbox={};this.hasHTML5=null;this.html5={usingFlash:null};this.ignoreFlash=false;var ba,b=this,z,t=navigator.userAgent,U=T.location.href.toString(),o=this.flashVersion,k=document,l=T,qa,V,D=[],L=false,M=false,q=false,A=false,ra=false,N,s,sa,E,F,ca,W,ta,da,B,ua,O,G,ea,fa,X,ga,ha,va,wa,P,xa,Q=null,ia=null,
H,ja,I,Y,Z,ka,p,$=false,la=false,ya,za,J=null,Aa,aa,C=false,R,x,ma,Ba,y,v,Ca=Array.prototype.slice,Da=t.match(/pre\//i),Ia=t.match(/(ipad|iphone)/i);t.match(/mobile/i);var w=t.match(/MSIE/i),S=t.match(/safari/i)&&!t.match(/chrome/i),na=typeof k.hasFocus!=="undefined"?k.hasFocus():null,K=typeof k.hasFocus==="undefined"&&S,Ea=!K;this._use_maybe=U.match(/sm2\-useHTML5Maybe\=1/i);this._overHTTP=k.location?k.location.protocol.match(/http/i):null;this.useAltURL=!this._overHTTP;if(Ia||Da){b.useHTML5Audio=
true;b.ignoreFlash=true}if(Da||this._use_maybe)b.html5Test=/^(probably|maybe)$/i;this.supported=function(){return J?q&&!A:b.useHTML5Audio&&b.hasHTML5};this.getMovie=function(c){return w?l[c]:S?z(c)||k[c]:z(c)};this.loadFromXML=function(c){try{b.o._loadFromXML(c)}catch(a){P()}return true};this.createSound=function(c){function a(){f=Y(f);b.sounds[e.id]=new ba(e);b.soundIDs.push(e.id);return b.sounds[e.id]}var f=null,i=null,e=null;if(!q||!b.supported()){ka("soundManager.createSound(): "+H(!q?"notReady":
"notOK"));return false}if(arguments.length===2)c={id:arguments[0],url:arguments[1]};e=f=s(c);if(p(e.id,true))return b.sounds[e.id];if(aa(e)){i=a();i._setup_html5(e)}else{if(o>8&&b.useMovieStar){if(e.isMovieStar===null)e.isMovieStar=e.serverURL||(e.type?e.type.match(b.netStreamPattern):false)||e.url.match(b.netStreamPattern)?true:false;if(e.isMovieStar)if(e.usePeakData)e.usePeakData=false}e=Z(e,"soundManager.createSound(): ");i=a();if(o===8)b.o._createSound(e.id,e.onjustbeforefinishtime,e.loops||1,
e.usePolicyFile);else{b.o._createSound(e.id,e.url,e.onjustbeforefinishtime,e.usePeakData,e.useWaveformData,e.useEQData,e.isMovieStar,e.isMovieStar?e.bufferTime:false,e.loops||1,e.serverURL,e.duration||null,e.autoPlay,true,e.bufferTimes,e.onstats?true:false,e.autoLoad,e.usePolicyFile);if(!e.serverURL){i.connected=true;e.onconnect&&e.onconnect.apply(i)}}}if(e.autoLoad||e.autoPlay)if(i)if(b.isHTML5){i.autobuffer="auto";i.preload="auto"}else i.load(e);e.autoPlay&&i.play();return i};this.destroySound=
function(c,a){if(!p(c))return false;var f=b.sounds[c],i;f._iO={};f.stop();f.unload();for(i=0;i<b.soundIDs.length;i++)if(b.soundIDs[i]===c){b.soundIDs.splice(i,1);break}a||f.destruct(true);delete b.sounds[c];return true};this.load=function(c,a){if(!p(c))return false;return b.sounds[c].load(a)};this.unload=function(c){if(!p(c))return false;return b.sounds[c].unload()};this.start=this.play=function(c,a){if(!q||!b.supported()){ka("soundManager.play(): "+H(!q?"notReady":"notOK"));return false}if(!p(c)){a instanceof
Object||(a={url:a});if(a&&a.url){a.id=c;return b.createSound(a).play()}else return false}return b.sounds[c].play(a)};this.setPosition=function(c,a){if(!p(c))return false;return b.sounds[c].setPosition(a)};this.stop=function(c){if(!p(c))return false;return b.sounds[c].stop()};this.stopAll=function(){for(var c in b.sounds)b.sounds[c]instanceof ba&&b.sounds[c].stop()};this.pause=function(c){if(!p(c))return false;return b.sounds[c].pause()};this.pauseAll=function(){for(var c=b.soundIDs.length;c--;)b.sounds[b.soundIDs[c]].pause()};
this.resume=function(c){if(!p(c))return false;return b.sounds[c].resume()};this.resumeAll=function(){for(var c=b.soundIDs.length;c--;)b.sounds[b.soundIDs[c]].resume()};this.togglePause=function(c){if(!p(c))return false;return b.sounds[c].togglePause()};this.setPan=function(c,a){if(!p(c))return false;return b.sounds[c].setPan(a)};this.setVolume=function(c,a){if(!p(c))return false;return b.sounds[c].setVolume(a)};this.mute=function(c){var a=0;if(typeof c!=="string")c=null;if(c){if(!p(c))return false;
return b.sounds[c].mute()}else{for(a=b.soundIDs.length;a--;)b.sounds[b.soundIDs[a]].mute();b.muted=true}return true};this.muteAll=function(){b.mute()};this.unmute=function(c){if(typeof c!=="string")c=null;if(c){if(!p(c))return false;return b.sounds[c].unmute()}else{for(c=b.soundIDs.length;c--;)b.sounds[b.soundIDs[c]].unmute();b.muted=false}return true};this.unmuteAll=function(){b.unmute()};this.toggleMute=function(c){if(!p(c))return false;return b.sounds[c].toggleMute()};this.getMemoryUse=function(){if(o===
8)return 0;if(b.o)return parseInt(b.o._getMemoryUse(),10)};this.disable=function(c){if(typeof c==="undefined")c=false;if(A)return false;A=true;for(var a=b.soundIDs.length;a--;)wa(b.sounds[b.soundIDs[a]]);N(c);v(l,"load",F);return true};this.canPlayMIME=function(c){var a;if(b.hasHTML5)a=R({type:c});return!J||a?a:c?c.match(b.mimePattern)?true:false:null};this.canPlayURL=function(c){var a;if(b.hasHTML5)a=R(c);return!J||a?a:c?c.match(b.filePattern)?true:false:null};this.canPlayLink=function(c){if(typeof c.type!==
"undefined"&&c.type)if(b.canPlayMIME(c.type))return true;return b.canPlayURL(c.href)};this.getSoundById=function(c){if(!c)throw Error("SoundManager.getSoundById(): sID is null/undefined");return b.sounds[c]};this.onready=function(c,a){if(c&&c instanceof Function){a||(a=l);sa(c,a);E();return true}else throw H("needFunction");};this.getMoviePercent=function(){return b.o&&typeof b.o.PercentLoaded!=="undefined"?b.o.PercentLoaded():null};this._wD=this._writeDebug=function(c,a,f){b.ondebuglog&&b.ondebuglog(c,
a,f);return true};this._debug=function(){};this.reboot=function(){for(var c=b.soundIDs.length;c--;)b.sounds[b.soundIDs[c]].destruct();try{if(w)ia=b.o.innerHTML;Q=b.o.parentNode.removeChild(b.o)}catch(a){}ia=Q=null;b.enabled=q=$=la=L=M=A=b.swfLoaded=false;b.soundIDs=b.sounds=[];b.o=null;for(c=D.length;c--;)D[c].fired=false;l.setTimeout(function(){b.beginDelayedInit()},20)};this.destruct=function(){b.disable(true)};this.beginDelayedInit=function(){ra=true;G();setTimeout(ua,20);W()};ba=function(c){var a=
this,f,i,e,j,n,h;this.sID=c.id;this.url=c.url;this._iO=this.instanceOptions=this.options=s(c);this.pan=this.options.pan;this.volume=this.options.volume;this._lastURL=null;this.isHTML5=false;this.id3={};this._debug=function(){};this._debug();this.load=function(d){var g=null;if(typeof d!=="undefined"){a._iO=s(d);a.instanceOptions=a._iO}else{d=a.options;a._iO=d;a.instanceOptions=a._iO;if(a._lastURL&&a._lastURL!==a.url){a._iO.url=a.url;a.url=null}}if(a._iO.url===a.url&&a.readyState!==0&&a.readyState!==
2)return a;a._lastURL=a.url;a.loaded=false;a.readyState=1;a.playState=0;if(aa(a._iO)){g=a._setup_html5(a._iO);g.load();a._iO.autoPlay&&a.play()}else try{a.isHTML5=false;a._iO=Z(Y(a._iO));o===8?b.o._load(a.sID,a._iO.url,a._iO.stream,a._iO.autoPlay,a._iO.whileloading?1:0,a._iO.loops||1,a._iO.usePolicyFile):b.o._load(a.sID,a._iO.url,a._iO.stream?true:false,a._iO.autoPlay?true:false,a._iO.loops||1,a._iO.autoLoad?true:false,a._iO.usePolicyFile)}catch(m){ga()}return a};this.unload=function(){if(a.readyState!==
0){if(a.isHTML5){e();if(h){h.pause();h.src=b.nullURL;h.load();h=a._audio=null}}else o===8?b.o._unload(a.sID,b.nullURL):b.o._unload(a.sID);f()}return a};this.destruct=function(d){if(a.isHTML5){e();if(h){h.pause();h.src="about:blank";h.load();h=a._audio=null}}else{a._iO.onfailure=null;b.o._destroySound(a.sID)}d||b.destroySound(a.sID,true)};this.start=this.play=function(d,g){var m;g=typeof g==="undefined"?true:g;d||(d={});a._iO=s(d,a._iO);a._iO=s(a._iO,a.options);a.instanceOptions=a._iO;if(a._iO.serverURL)if(!a.connected){a.getAutoPlay()||
a.setAutoPlay(true);return a}if(aa(a._iO)){a._setup_html5(a._iO);j()}if(a.playState===1&&!a.paused)if(m=a._iO.multiShot)a.isHTML5&&a.setPosition(a._iO.position);else return a;if(!a.loaded)if(a.readyState===0)if(a.isHTML5){a.load(a._iO);a.readyState=1}else{if(!a._iO.serverURL){a._iO.autoPlay=true;a.load(a._iO)}}else if(a.readyState===2)return a;if(a.paused&&a.position&&a.position>0)a.resume();else{a.playState=1;a.paused=false;if(!a.instanceCount||a._iO.multiShotEvents||o>8&&!a.isHTML5&&!a.getAutoPlay())a.instanceCount++;
a.position=typeof a._iO.position!=="undefined"&&!isNaN(a._iO.position)?a._iO.position:0;a._iO=Z(Y(a._iO));a._iO.onplay&&g&&a._iO.onplay.apply(a);a.setVolume(a._iO.volume,true);a.setPan(a._iO.pan,true);if(a.isHTML5){j();a._setup_html5().play()}else b.o._start(a.sID,a._iO.loops||1,o===9?a.position:a.position/1E3)}return a};this.stop=function(d){if(a.playState===1){a._onbufferchange(0);a.resetOnPosition(0);if(!a.isHTML5)a.playState=0;a.paused=false;a._iO.onstop&&a._iO.onstop.apply(a);if(a.isHTML5){if(h){a.setPosition(0);
h.pause();a.playState=0;a._onTimer();e();a.unload()}}else{b.o._stop(a.sID,d);a._iO.serverURL&&a.unload()}a.instanceCount=0;a._iO={}}return a};this.setAutoPlay=function(d){a._iO.autoPlay=d;b.o._setAutoPlay(a.sID,d);d&&!a.instanceCount&&a.readyState===1&&a.instanceCount++};this.getAutoPlay=function(){return a._iO.autoPlay};this.setPosition=function(d){if(d===undefined)d=0;a.position=a.isHTML5?Math.max(d,0):Math.min(a.duration||a._iO.duration,Math.max(d,0));a.resetOnPosition(a.position);if(a.isHTML5){if(h){if(a.playState)try{h.currentTime=
a.position/1E3}catch(g){}if(a.paused){a._onTimer(true);a._iO.useMovieStar&&a.resume()}}}else{d=o===9?a.position:a.position/1E3;a.playState===0?a.play({position:d}):b.o._setPosition(a.sID,d,a.paused||!a.playState)}return a};this.pause=function(d){if(a.paused||a.playState===0&&a.readyState!==1)return a;a.paused=true;if(a.isHTML5){a._setup_html5().pause();e()}else if(d||d===undefined)b.o._pause(a.sID);a._iO.onpause&&a._iO.onpause.apply(a);return a};this.resume=function(){if(!a.paused)return a;a.paused=
false;a.playState=1;if(a.isHTML5){a._setup_html5().play();j()}else b.o._pause(a.sID);a._iO.onresume&&a._iO.onresume.apply(a);return a};this.togglePause=function(){if(a.playState===0){a.play({position:o===9&&!a.isHTML5?a.position:a.position/1E3});return a}a.paused?a.resume():a.pause();return a};this.setPan=function(d,g){if(typeof d==="undefined")d=0;if(typeof g==="undefined")g=false;a.isHTML5||b.o._setPan(a.sID,d);a._iO.pan=d;if(!g)a.pan=d;return a};this.setVolume=function(d,g){if(typeof d==="undefined")d=
100;if(typeof g==="undefined")g=false;if(a.isHTML5){if(h)h.volume=d/100}else b.o._setVolume(a.sID,b.muted&&!a.muted||a.muted?0:d);a._iO.volume=d;if(!g)a.volume=d;return a};this.mute=function(){a.muted=true;if(a.isHTML5){if(h)h.muted=true}else b.o._setVolume(a.sID,0);return a};this.unmute=function(){a.muted=false;var d=typeof a._iO.volume!=="undefined";if(a.isHTML5){if(h)h.muted=false}else b.o._setVolume(a.sID,d?a._iO.volume:a.options.volume);return a};this.toggleMute=function(){return a.muted?a.unmute():
a.mute()};this.onposition=function(d,g,m){a._onPositionItems.push({position:d,method:g,scope:typeof m!=="undefined"?m:a,fired:false});return a};this.processOnPosition=function(){var d,g;d=a._onPositionItems.length;if(!d||!a.playState||a._onPositionFired>=d)return false;for(d=d;d--;){g=a._onPositionItems[d];if(!g.fired&&a.position>=g.position){g.method.apply(g.scope,[g.position]);g.fired=true;b._onPositionFired++}}return true};this.resetOnPosition=function(d){var g,m;g=a._onPositionItems.length;if(!g)return false;
for(g=g;g--;){m=a._onPositionItems[g];if(m.fired&&d<=m.position){m.fired=false;b._onPositionFired--}}return true};this._onTimer=function(d){var g={};if(a._hasTimer||d)if(h&&(d||(a.playState>0||a.readyState===1)&&!a.paused)){a.duration=n();a.durationEstimate=a.duration;d=h.currentTime?h.currentTime*1E3:0;a._whileplaying(d,g,g,g,g);return true}else return false};n=function(){var d=h?h.duration*1E3:undefined;return d&&!isNaN(d)?d:null};j=function(){a.isHTML5&&ya(a)};e=function(){a.isHTML5&&za(a)};f=
function(){a._onPositionItems=[];a._onPositionFired=0;a._hasTimer=null;a._added_events=null;h=a._audio=null;a.bytesLoaded=null;a.bytesTotal=null;a.position=null;a.duration=a._iO&&a._iO.duration?a._iO.duration:null;a.durationEstimate=null;a.failures=0;a.loaded=false;a.playState=0;a.paused=false;a.readyState=0;a.muted=false;a.didBeforeFinish=false;a.didJustBeforeFinish=false;a.isBuffering=false;a.instanceOptions={};a.instanceCount=0;a.peakData={left:0,right:0};a.waveformData={left:[],right:[]};a.eqData=
[];a.eqData.left=[];a.eqData.right=[]};f();this._setup_html5=function(d){d=s(a._iO,d);if(h){if(a.url!==d.url)h.src=d.url}else{a._audio=new Audio(d.url);h=a._audio;a.isHTML5=true;i()}h.loop=d.loops>1?"loop":"";return a._audio};i=function(){function d(g,m,r){return h?h.addEventListener(g,m,r||false):null}if(a._added_events)return false;a._added_events=true;d("load",function(){if(h){a._onbufferchange(0);a._whileloading(a.bytesTotal,a.bytesTotal,n());a._onload(true)}},false);d("canplay",function(){a._onbufferchange(0)},
false);d("waiting",function(){a._onbufferchange(1)},false);d("progress",function(g){if(!a.loaded&&h){a._onbufferchange(0);a._whileloading(g.loaded||0,g.total||1,n())}},false);d("error",function(){h&&a._onload(false)},false);d("loadstart",function(){a._onbufferchange(1)},false);d("play",function(){a._onbufferchange(0)},false);d("playing",function(){a._onbufferchange(0)},false);d("timeupdate",function(){a._onTimer()},false);setTimeout(function(){a&&h&&d("ended",function(){a._onfinish()},false)},250);
return true};this._whileloading=function(d,g,m,r){a.bytesLoaded=d;a.bytesTotal=g;a.duration=Math.floor(m);a.bufferLength=r;if(a._iO.isMovieStar)a.durationEstimate=a.duration;else{a.durationEstimate=a._iO.duration?a.duration>a._iO.duration?a.duration:a._iO.duration:parseInt(a.bytesTotal/a.bytesLoaded*a.duration,10);if(a.durationEstimate===undefined)a.durationEstimate=a.duration;a.bufferLength=r}a.readyState!==3&&a._iO.whileloading&&a._iO.whileloading.apply(a)};this._onid3=function(d,g){var m=[],r,
u;r=0;for(u=d.length;r<u;r++)m[d[r]]=g[r];a.id3=s(a.id3,m);a._iO.onid3&&a._iO.onid3.apply(a)};this._whileplaying=function(d,g,m,r,u){if(isNaN(d)||d===null)return false;if(a.playState===0&&d>0)d=0;a.position=d;a.processOnPosition();if(o>8&&!a.isHTML5){if(a._iO.usePeakData&&typeof g!=="undefined"&&g)a.peakData={left:g.leftPeak,right:g.rightPeak};if(a._iO.useWaveformData&&typeof m!=="undefined"&&m)a.waveformData={left:m.split(","),right:r.split(",")};if(a._iO.useEQData)if(typeof u!=="undefined"&&u&&
u.leftEQ){d=u.leftEQ.split(",");a.eqData=d;a.eqData.left=d;if(typeof u.rightEQ!=="undefined"&&u.rightEQ)a.eqData.right=u.rightEQ.split(",")}}if(a.playState===1){!a.isHTML5&&b.flashVersion===8&&!a.position&&a.isBuffering&&a._onbufferchange(0);a._iO.whileplaying&&a._iO.whileplaying.apply(a);if((a.loaded||!a.loaded&&a._iO.isMovieStar)&&a._iO.onbeforefinish&&a._iO.onbeforefinishtime&&!a.didBeforeFinish&&a.duration-a.position<=a._iO.onbeforefinishtime)a._onbeforefinish()}return true};this._onconnect=function(d){d=
d===1;if(a.connected=d){a.failures=0;a._iO.onconnect&&a._iO.onconnect.apply(a,[d]);if(p(a.sID)&&(a.options.autoLoad||a.getAutoPlay()))a.play(undefined,a.getAutoPlay())}};this._onload=function(d){d=d?true:false;a.loaded=d;a.readyState=d?3:2;a._onbufferchange(0);a._iO.onload&&a._iO.onload.apply(a,[d]);return true};this._onfailure=function(d,g,m){a.failures++;a._iO.onfailure&&a.failures===1&&a._iO.onfailure(a,d,g,m)};this._onbeforefinish=function(){if(!a.didBeforeFinish){a.didBeforeFinish=true;a._iO.onbeforefinish&&
a._iO.onbeforefinish.apply(a)}};this._onjustbeforefinish=function(){if(!a.didJustBeforeFinish){a.didJustBeforeFinish=true;a._iO.onjustbeforefinish&&a._iO.onjustbeforefinish.apply(a)}};this._onstats=function(d){a._iO.onstats&&a._iO.onstats(a,d)};this._onfinish=function(){a._onbufferchange(0);a.resetOnPosition(0);a._iO.onbeforefinishcomplete&&a._iO.onbeforefinishcomplete.apply(a);a.didBeforeFinish=false;a.didJustBeforeFinish=false;if(a.instanceCount){a.instanceCount--;if(!a.instanceCount){a.playState=
0;a.paused=false;a.instanceCount=0;a.instanceOptions={};e()}if(!a.instanceCount||a._iO.multiShotEvents)a._iO.onfinish&&a._iO.onfinish.apply(a)}};this._onbufferchange=function(d){if(a.playState===0)return false;if(d&&a.isBuffering||!d&&!a.isBuffering)return false;a.isBuffering=d===1;a._iO.onbufferchange&&a._iO.onbufferchange.apply(a);return true};this._ondataerror=function(){a.playState>0&&a._iO.ondataerror&&a._iO.ondataerror.apply(a)}};fa=function(){return k.body?k.body:k._docElement?k.documentElement:
k.getElementsByTagName("div")[0]};z=function(c){return k.getElementById(c)};s=function(c,a){var f={},i,e;for(i in c)if(c.hasOwnProperty(i))f[i]=c[i];i=typeof a==="undefined"?b.defaultOptions:a;for(e in i)if(i.hasOwnProperty(e)&&typeof f[e]==="undefined")f[e]=i[e];return f};(function(){function c(e){e=Ca.call(e);var j=e.length;if(f){e[1]="on"+e[1];j>3&&e.pop()}else j===3&&e.push(false);return e}function a(e,j){var n=e.shift()[i[j]];f?n(e[0],e[1]):n.apply(this,e)}var f=l.attachEvent,i={add:f?"attachEvent":
"addEventListener",remove:f?"detachEvent":"removeEventListener"};y=function(){a(c(arguments),"add")};v=function(){a(c(arguments),"remove")}})();aa=function(c){return(c.type?R({type:c.type}):false)||R(c.url)};R=function(c){if(!b.useHTML5Audio||!b.hasHTML5)return false;var a,f=b.audioFormats;if(!x){x=[];for(a in f)if(f.hasOwnProperty(a)){x.push(a);if(f[a].related)x=x.concat(f[a].related)}x=RegExp("\\.("+x.join("|")+")","i")}a=typeof c.type!=="undefined"?c.type:null;c=typeof c==="string"?c.toLowerCase().match(x):
null;if(!c||!c.length){if(!a)return false}else c=c[0].substr(1);if(c&&typeof b.html5[c]!=="undefined")return b.html5[c];else{if(!a)if(c&&b.html5[c])return b.html5[c];else a="audio/"+c;a=b.html5.canPlayType(a);return b.html5[c]=a}};Ba=function(){function c(n){var h,d,g=false;if(!a||typeof a.canPlayType!=="function")return false;if(n instanceof Array){h=0;for(d=n.length;h<d&&!g;h++)if(b.html5[n[h]]||a.canPlayType(n[h]).match(b.html5Test)){g=true;b.html5[n[h]]=true}return g}else return(n=a&&typeof a.canPlayType===
"function"?a.canPlayType(n):false)&&(n.match(b.html5Test)?true:false)}if(!b.useHTML5Audio||typeof Audio==="undefined")return false;var a=typeof Audio!=="undefined"?new Audio:null,f,i={},e,j;e=b.audioFormats;for(f in e)if(e.hasOwnProperty(f)){i[f]=c(e[f].type);if(e[f]&&e[f].related)for(j=0;j<e[f].related.length;j++)b.html5[e[f].related[j]]=i[f]}i.canPlayType=a?c:null;b.html5=s(b.html5,i);return true};z=function(c){return k.getElementById(c)};H=function(){};Y=function(c){if(o===8&&c.loops>1&&c.stream)c.stream=
false;return c};Z=function(c){if(c&&!c.usePolicyFile&&(c.onid3||c.usePeakData||c.useWaveformData||c.useEQData))c.usePolicyFile=true;return c};ka=function(c){typeof console!=="undefined"&&typeof console.warn!=="undefined"&&console.warn(c)};qa=function(){return false};wa=function(c){for(var a in c)if(c.hasOwnProperty(a)&&typeof c[a]==="function")c[a]=qa};P=function(c){if(typeof c==="undefined")c=false;if(A||c)b.disable(c)};xa=function(c){var a=null;if(c)if(c.match(/\.swf(\?\.*)?$/i)){if(a=c.substr(c.toLowerCase().lastIndexOf(".swf?")+
4))return c}else if(c.lastIndexOf("/")!==c.length-1)c+="/";return(c&&c.lastIndexOf("/")!==-1?c.substr(0,c.lastIndexOf("/")+1):"./")+b.movieURL};da=function(){if(o!==8&&o!==9)b.flashVersion=8;var c=b.debugMode||b.debugFlash?"_debug.swf":".swf";if(b.flashVersion<9&&b.useHTML5Audio&&b.audioFormats.mp4.required)b.flashVersion=9;o=b.flashVersion;b.version=b.versionNumber+(C?" (HTML5-only mode)":o===9?" (AS3/Flash 9)":" (AS2/Flash 8)");if(o>8){b.defaultOptions=s(b.defaultOptions,b.flash9Options);b.features.buffering=
true}if(o>8&&b.useMovieStar){b.defaultOptions=s(b.defaultOptions,b.movieStarOptions);b.filePatterns.flash9=RegExp("\\.(mp3|"+b.netStreamTypes.join("|")+")(\\?.*)?$","i");b.mimePattern=b.netStreamMimeTypes;b.features.movieStar=true}else b.features.movieStar=false;b.filePattern=b.filePatterns[o!==8?"flash9":"flash8"];b.movieURL=(o===8?"soundmanager2.swf":"soundmanager2_flash9.swf").replace(".swf",c);b.features.peakData=b.features.waveformData=b.features.eqData=o>8};va=function(c,a){if(!b.o||!b.allowPolling)return false;
b.o._setPolling(c,a)};(function(){function c(e){e=Ca.call(e);var j=e.length;if(f){e[1]="on"+e[1];j>3&&e.pop()}else j===3&&e.push(false);return e}function a(e,j){var n=e.shift()[i[j]];f?n(e[0],e[1]):n.apply(this,e)}var f=l.attachEvent,i={add:f?"attachEvent":"addEventListener",remove:f?"detachEvent":"removeEventListener"};y=function(){a(c(arguments),"add")};v=function(){a(c(arguments),"remove")}})();ha=function(){function c(){f.left=l.scrollX+"px";f.top=l.scrollY+"px"}function a(i){i=l[(i?"add":"remove")+
"EventListener"];i("resize",c,false);i("scroll",c,false)}var f=null;return{check:function(i){f=b.oMC.style;if(t.match(/android/i)){if(i){if(b.flashLoadTimeout)b._s.flashLoadTimeout=0;return false}f.position="absolute";f.left=f.top="0px";a(true);b.onready(function(){a(false);if(f)f.left=f.top="-9999px"});c()}return true}}}();X=function(c,a){var f=a?a:b.url,i=b.altURL?b.altURL:f,e;e=fa();var j,n,h=I(),d,g=null;g=(g=k.getElementsByTagName("html")[0])&&g.dir&&g.dir.match(/rtl/i);c=typeof c==="undefined"?
b.id:c;if(L&&M)return false;if(C){da();b.oMC=z(b.movieID);V();M=L=true;return false}L=true;da();b.url=xa(this._overHTTP?f:i);a=b.url;b.wmode=!b.wmode&&b.useHighPerformance&&!b.useMovieStar?"transparent":b.wmode;if(b.wmode!==null&&!w&&!b.useHighPerformance&&navigator.platform.match(/win32/i)){b.specialWmodeCase=true;b.wmode=null}e={name:c,id:c,src:a,width:"100%",height:"100%",quality:"high",allowScriptAccess:b.allowScriptAccess,bgcolor:b.bgColor,pluginspage:"http://www.macromedia.com/go/getflashplayer",
type:"application/x-shockwave-flash",wmode:b.wmode};if(b.debugFlash)e.FlashVars="debug=1";b.wmode||delete e.wmode;if(w){f=k.createElement("div");n='<object id="'+c+'" data="'+a+'" type="'+e.type+'" width="'+e.width+'" height="'+e.height+'"><param name="movie" value="'+a+'" /><param name="AllowScriptAccess" value="'+b.allowScriptAccess+'" /><param name="quality" value="'+e.quality+'" />'+(b.wmode?'<param name="wmode" value="'+b.wmode+'" /> ':"")+'<param name="bgcolor" value="'+b.bgColor+'" />'+(b.debugFlash?
'<param name="FlashVars" value="'+e.FlashVars+'" />':"")+"<!-- --\></object>"}else{f=k.createElement("embed");for(j in e)e.hasOwnProperty(j)&&f.setAttribute(j,e[j])}pa();h=I();if(e=fa()){b.oMC=z(b.movieID)?z(b.movieID):k.createElement("div");if(b.oMC.id){d=b.oMC.className;b.oMC.className=(d?d+" ":b.swfCSS.swfDefault)+(h?" "+h:"");b.oMC.appendChild(f);if(w){j=b.oMC.appendChild(k.createElement("div"));j.className=b.swfCSS.swfBox;j.innerHTML=n}M=true;ha.check(true)}else{b.oMC.id=b.movieID;b.oMC.className=
b.swfCSS.swfDefault+" "+h;j=h=null;if(!b.useFlashBlock)if(b.useHighPerformance)h={position:"fixed",width:"8px",height:"8px",bottom:"0px",left:"0px",overflow:"hidden"};else{h={position:"absolute",width:"6px",height:"6px",top:"-9999px",left:"-9999px"};if(g)h.left=Math.abs(parseInt(h.left,10))+"px"}if(t.match(/webkit/i))b.oMC.style.zIndex=1E4;if(!b.debugFlash)for(d in h)if(h.hasOwnProperty(d))b.oMC.style[d]=h[d];try{w||b.oMC.appendChild(f);e.appendChild(b.oMC);if(w){j=b.oMC.appendChild(k.createElement("div"));
j.className=b.swfCSS.swfBox;j.innerHTML=n}M=true}catch(m){throw Error(H("appXHTML"));}ha.check()}}return true};p=this.getSoundById;O=function(){if(C){X();return false}if(b.o)return false;b.o=b.getMovie(b.id);if(!b.o){if(Q){if(w)b.oMC.innerHTML=ia;else b.oMC.appendChild(Q);Q=null;L=true}else X(b.id,b.url);b.o=b.getMovie(b.id)}b.oninitmovie instanceof Function&&setTimeout(b.oninitmovie,1);return true};ca=function(c){if(c)b.url=c;O()};W=function(){setTimeout(ta,500)};ta=function(){if($)return false;
$=true;v(l,"load",W);if(K&&!na)return false;var c;q||(c=b.getMoviePercent());setTimeout(function(){c=b.getMoviePercent();if(!q&&Ea)if(c===null)if(b.useFlashBlock||b.flashLoadTimeout===0)b.useFlashBlock&&ja();else P(true);else b.flashLoadTimeout!==0&&P(true)},b.flashLoadTimeout)};ca=function(c){if(c)b.url=c;O()};I=function(){var c=[];b.debugMode&&c.push(b.swfCSS.sm2Debug);b.debugFlash&&c.push(b.swfCSS.flashDebug);b.useHighPerformance&&c.push(b.swfCSS.highPerf);return c.join(" ")};ja=function(){H("fbHandler");
var c=b.getMoviePercent();if(b.supported()){if(b.oMC)b.oMC.className=I()+" "+b.swfCSS.swfDefault+(" "+b.swfCSS.swfUnblocked)}else{if(J)b.oMC.className=I()+" "+b.swfCSS.swfDefault+" "+(c===null?b.swfCSS.swfTimedout:b.swfCSS.swfError);b.didFlashBlock=true;E(true);b.onerror instanceof Function&&b.onerror.apply(l)}};B=function(){function c(){v(l,"focus",B);v(l,"load",B)}if(na||!K){c();return true}na=Ea=true;S&&K&&v(l,"mousemove",B);$=false;c();return true};N=function(c){if(q)return false;if(C){q=true;
E();F();return true}b.useFlashBlock&&b.flashLoadTimeout&&!b.getMoviePercent()||(q=true);if(A||c){if(b.useFlashBlock)b.oMC.className=I()+" "+(b.getMoviePercent()===null?b.swfCSS.swfTimedout:b.swfCSS.swfError);E();b.onerror instanceof Function&&b.onerror.apply(l);return false}if(b.waitForWindowLoad&&!ra){y(l,"load",F);return false}else F();return true};sa=function(c,a){D.push({method:c,scope:a||null,fired:false})};E=function(c){if(!q&&!c)return false;c={success:c?b.supported():!A};var a=[],f,i,e=!b.useFlashBlock||
b.useFlashBlock&&!b.supported();f=0;for(i=D.length;f<i;f++)D[f].fired!==true&&a.push(D[f]);if(a.length){f=0;for(i=a.length;f<i;f++){a[f].scope?a[f].method.apply(a[f].scope,[c]):a[f].method(c);if(!e)a[f].fired=true}}return true};F=function(){l.setTimeout(function(){b.useFlashBlock&&ja();E();b.onload instanceof Function&&b.onload.apply(l);b.waitForWindowLoad&&y(l,"load",F)},1)};Aa=function(){var c,a,f=!U.match(/usehtml5audio/i)&&!U.match(/sm2\-ignorebadua/i)&&S&&t.match(/OS X 10_6_(3|4)/i);if(t.match(/iphone os (1|2|3_0|3_1)/i)?
true:false){b.hasHTML5=false;C=true;if(b.oMC)b.oMC.style.display="none";return false}if(b.useHTML5Audio){if(!b.html5||!b.html5.canPlayType){b.hasHTML5=false;return true}else b.hasHTML5=true;if(f){b.useHTML5Audio=false;b.hasHTML5=false;return true}}else return true;for(a in b.audioFormats)if(b.audioFormats.hasOwnProperty(a)&&b.audioFormats[a].required&&!b.html5.canPlayType(b.audioFormats[a].type))c=true;if(b.ignoreFlash)c=false;C=b.useHTML5Audio&&b.hasHTML5&&!c;return c};V=function(){var c,a=[];if(q)return false;
if(b.hasHTML5)for(c in b.audioFormats)b.audioFormats.hasOwnProperty(c)&&a.push(c+": "+b.html5[c]);if(C){if(!q){v(l,"load",b.beginDelayedInit);b.enabled=true;N()}return true}O();try{b.o._externalInterfaceTest(false);if(b.allowPolling)va(true,b.useFastPolling?true:false);b.debugMode||b.o._disableDebug();b.enabled=true}catch(f){P(true);N();return false}N();v(l,"load",b.beginDelayedInit);return true};ua=function(){if(la)return false;X();O();return la=true};G=function(){if(ea)return false;ea=true;pa();
Ba();b.html5.usingFlash=Aa();J=b.html5.usingFlash;ea=true;k.removeEventListener&&k.removeEventListener("DOMContentLoaded",G,false);ca();return true};ya=function(c){if(!c._hasTimer)c._hasTimer=true};za=function(c){if(c._hasTimer)c._hasTimer=false};ga=function(){b.onerror instanceof Function&&b.onerror();b.disable()};this._setSandboxType=function(){};this._externalInterfaceOK=function(){if(b.swfLoaded)return false;(new Date).getTime();b.swfLoaded=true;K=false;w?setTimeout(V,100):V()};ma=function(){if(k.readyState===
"complete"){G();k.detachEvent("onreadystatechange",ma)}return true};if(!b.hasHTML5||J){y(l,"focus",B);y(l,"load",B);y(l,"load",W);S&&K&&y(l,"mousemove",B)}if(k.addEventListener)k.addEventListener("DOMContentLoaded",G,false);else k.attachEvent?k.attachEvent("onreadystatechange",ma):ga();k.readyState==="complete"&&setTimeout(G,100)}var Fa=null;if(typeof SM2_DEFER==="undefined"||!SM2_DEFER)Fa=new oa;T.SoundManager=oa;T.soundManager=Fa})(window);

(function(){var c=0,h=[],j={},f={},a={"<":"lt",">":"gt","&":"amp",'"':"quot","'":"#39"},i=/[<>&\"\']/g,b;function e(){this.returnValue=false}function g(){this.cancelBubble=true}(function(k){var l=k.split(/,/),m,o,n;for(m=0;m<l.length;m+=2){n=l[m+1].split(/ /);for(o=0;o<n.length;o++){f[n[o]]=l[m]}}})("application/msword,doc dot,application/pdf,pdf,application/pgp-signature,pgp,application/postscript,ps ai eps,application/rtf,rtf,application/vnd.ms-excel,xls xlb,application/vnd.ms-powerpoint,ppt pps pot,application/zip,zip,application/x-shockwave-flash,swf swfl,application/vnd.openxmlformats,docx pptx xlsx,audio/mpeg,mpga mpega mp2 mp3,audio/x-wav,wav,image/bmp,bmp,image/gif,gif,image/jpeg,jpeg jpg jpe,image/png,png,image/svg+xml,svg svgz,image/tiff,tiff tif,text/html,htm html xhtml,text/rtf,rtf,video/mpeg,mpeg mpg mpe,video/quicktime,qt mov,video/x-flv,flv,video/vnd.rn-realvideo,rv,text/plain,asc txt text diff log,application/octet-stream,exe");var d={STOPPED:1,STARTED:2,QUEUED:1,UPLOADING:2,FAILED:4,DONE:5,GENERIC_ERROR:-100,HTTP_ERROR:-200,IO_ERROR:-300,SECURITY_ERROR:-400,INIT_ERROR:-500,FILE_SIZE_ERROR:-600,FILE_EXTENSION_ERROR:-700,mimeTypes:f,extend:function(k){d.each(arguments,function(l,m){if(m>0){d.each(l,function(o,n){k[n]=o})}});return k},cleanName:function(k){var l,m;m=[/[\300-\306]/g,"A",/[\340-\346]/g,"a",/\307/g,"C",/\347/g,"c",/[\310-\313]/g,"E",/[\350-\353]/g,"e",/[\314-\317]/g,"I",/[\354-\357]/g,"i",/\321/g,"N",/\361/g,"n",/[\322-\330]/g,"O",/[\362-\370]/g,"o",/[\331-\334]/g,"U",/[\371-\374]/g,"u"];for(l=0;l<m.length;l+=2){k=k.replace(m[l],m[l+1])}k=k.replace(/\s+/g,"_");k=k.replace(/[^a-z0-9_\-\.]+/gi,"");return k},addRuntime:function(k,l){l.name=k;h[k]=l;h.push(l);return l},guid:function(){var k=new Date().getTime().toString(32),l;for(l=0;l<5;l++){k+=Math.floor(Math.random()*65535).toString(32)}return(d.guidPrefix||"p")+k+(c++).toString(32)},buildUrl:function(l,k){var m="";d.each(k,function(o,n){m+=(m?"&":"")+encodeURIComponent(n)+"="+encodeURIComponent(o)});if(m){l+=(l.indexOf("?")>0?"&":"?")+m}return l},each:function(n,o){var m,l,k;if(n){m=n.length;if(m===b){for(l in n){if(n.hasOwnProperty(l)){if(o(n[l],l)===false){return}}}}else{for(k=0;k<m;k++){if(o(n[k],k)===false){return}}}}},formatSize:function(k){if(k===b){return d.translate("N/A")}if(k>1048576){return Math.round(k/1048576,1)+" MB"}if(k>1024){return Math.round(k/1024,1)+" KB"}return k+" b"},getPos:function(l,p){var q=0,o=0,s,r=document,m,n;l=l;p=p||r.body;function k(w){var u,v,t=0,z=0;if(w){v=w.getBoundingClientRect();u=r.compatMode==="CSS1Compat"?r.documentElement:r.body;t=v.left+u.scrollLeft;z=v.top+u.scrollTop}return{x:t,y:z}}if(l.getBoundingClientRect&&(navigator.userAgent.indexOf("MSIE")>0&&r.documentMode!==8)){m=k(l);n=k(p);return{x:m.x-n.x,y:m.y-n.y}}s=l;while(s&&s!=p&&s.nodeType){q+=s.offsetLeft||0;o+=s.offsetTop||0;s=s.offsetParent}s=l.parentNode;while(s&&s!=p&&s.nodeType){q-=s.scrollLeft||0;o-=s.scrollTop||0;s=s.parentNode}return{x:q,y:o}},getSize:function(k){return{w:k.clientWidth||k.offsetWidth,h:k.clientHeight||k.offsetHeight}},parseSize:function(k){var l;if(typeof(k)=="string"){k=/^([0-9]+)([mgk]+)$/.exec(k.toLowerCase().replace(/[^0-9mkg]/g,""));l=k[2];k=+k[1];if(l=="g"){k*=1073741824}if(l=="m"){k*=1048576}if(l=="k"){k*=1024}}return k},xmlEncode:function(k){return k?(""+k).replace(i,function(l){return a[l]?"&"+a[l]+";":l}):k},toArray:function(m){var l,k=[];for(l=0;l<m.length;l++){k[l]=m[l]}return k},addI18n:function(k){return d.extend(j,k)},translate:function(k){return j[k]||k},addEvent:function(l,k,m){if(l.attachEvent){l.attachEvent("on"+k,function(){var n=window.event;if(!n.target){n.target=n.srcElement}n.preventDefault=e;n.stopPropagation=g;m(n)})}else{if(l.addEventListener){l.addEventListener(k,m,false)}}}};d.Uploader=function(n){var l={},q,p=[],r,m;q=new d.QueueProgress();n=d.extend({chunk_size:0,max_file_size:"1gb",multi_selection:true,file_data_name:"file",filters:[]},n);function o(){var s;if(this.state==d.STARTED&&r<p.length){s=p[r++];if(s.status==d.QUEUED){this.trigger("UploadFile",s)}else{o.call(this)}}else{this.stop()}}function k(){var t,s;q.reset();for(t=0;t<p.length;t++){s=p[t];if(s.size!==b){q.size+=s.size;q.loaded+=s.loaded}else{q.size=b}if(s.status==d.DONE){q.uploaded++}else{if(s.status==d.FAILED){q.failed++}else{q.queued++}}}if(q.size===b){q.percent=p.length>0?Math.ceil(q.uploaded/p.length*100):0}else{q.bytesPerSec=Math.ceil(q.loaded/((+new Date()-m||1)/1000));q.percent=q.size>0?Math.ceil(q.loaded/q.size*100):0}}d.extend(this,{state:d.STOPPED,features:{},files:p,settings:n,total:q,id:d.guid(),init:function(){var x=this,y,u,t,w=0,v;n.page_url=n.page_url||document.location.pathname.replace(/\/[^\/]+$/g,"/");if(!/^(\w+:\/\/|\/)/.test(n.url)){n.url=n.page_url+n.url}n.chunk_size=d.parseSize(n.chunk_size);n.max_file_size=d.parseSize(n.max_file_size);x.bind("FilesAdded",function(z,C){var B,A,F=0,E,D=n.filters;if(D&&D.length){E={};d.each(D,function(G){d.each(G.extensions.split(/,/),function(H){E[H.toLowerCase()]=true})})}for(B=0;B<C.length;B++){A=C[B];A.loaded=0;A.percent=0;A.status=d.QUEUED;if(E&&!E[A.name.toLowerCase().split(".").slice(-1)]){z.trigger("Error",{code:d.FILE_EXTENSION_ERROR,message:"File extension error.",file:A});continue}if(A.size!==b&&A.size>n.max_file_size){z.trigger("Error",{code:d.FILE_SIZE_ERROR,message:"File size error.",file:A});continue}p.push(A);F++}if(F){x.trigger("QueueChanged");x.refresh()}});if(n.unique_names){x.bind("UploadFile",function(z,A){A.target_name=A.id+".tmp"})}x.bind("UploadProgress",function(z,A){if(A.status==d.QUEUED){A.status=d.UPLOADING}A.percent=A.size>0?Math.ceil(A.loaded/A.size*100):100;k()});x.bind("StateChanged",function(z){if(z.state==d.STARTED){m=(+new Date())}});x.bind("QueueChanged",k);x.bind("Error",function(z,A){if(A.file){A.file.status=d.FAILED;k();window.setTimeout(function(){o.call(x)})}});x.bind("FileUploaded",function(z,A){A.status=d.DONE;z.trigger("UploadProgress",A);o.call(x)});if(n.runtimes){u=[];v=n.runtimes.split(/\s?,\s?/);for(y=0;y<v.length;y++){if(h[v[y]]){u.push(h[v[y]])}}}else{u=h}function s(){var C=u[w++],B,z,A;if(C){B=C.getFeatures();z=x.settings.required_features;if(z){z=z.split(",");for(A=0;A<z.length;A++){if(!B[z[A]]){s();return}}}C.init(x,function(D){if(D&&D.success){x.features=B;x.trigger("Init",{runtime:C.name});x.trigger("PostInit");x.refresh()}else{s()}})}else{x.trigger("Error",{code:d.INIT_ERROR,message:"Init error."})}}s()},refresh:function(){this.trigger("Refresh")},start:function(){if(this.state!=d.STARTED){r=0;this.state=d.STARTED;this.trigger("StateChanged");o.call(this)}},stop:function(){if(this.state!=d.STOPPED){this.state=d.STOPPED;this.trigger("StateChanged")}},getFile:function(t){var s;for(s=p.length-1;s>=0;s--){if(p[s].id===t){return p[s]}}},removeFile:function(t){var s;for(s=p.length-1;s>=0;s--){if(p[s].id===t.id){return this.splice(s,1)[0]}}},splice:function(u,s){var t;t=p.splice(u,s);this.trigger("FilesRemoved",t);this.trigger("QueueChanged");return t},trigger:function(t){var v=l[t.toLowerCase()],u,s;if(v){s=Array.prototype.slice.call(arguments);s[0]=this;for(u=0;u<v.length;u++){if(v[u].func.apply(v[u].scope,s)===false){return false}}}return true},bind:function(s,u,t){var v;s=s.toLowerCase();v=l[s]||[];v.push({func:u,scope:t||this});l[s]=v},unbind:function(s,u){var v=l[s.toLowerCase()],t;if(v){for(t=v.length-1;t>=0;t--){if(v[t].func===u){v.splice(t,1)}}}}})};d.File=function(n,l,m){var k=this;k.id=n;k.name=l;k.size=m;k.loaded=0;k.percent=0;k.status=0};d.Runtime=function(){this.getFeatures=function(){};this.init=function(k,l){}};d.QueueProgress=function(){var k=this;k.size=0;k.loaded=0;k.uploaded=0;k.failed=0;k.queued=0;k.percent=0;k.bytesPerSec=0;k.reset=function(){k.size=k.loaded=k.uploaded=k.failed=k.queued=k.percent=k.bytesPerSec=0}};d.runtimes={};window.plupload=d})();(function(b){var c={};function a(i,e,k,j,d){var l,g,f,h;g=google.gears.factory.create("beta.canvas");g.decode(i);h=Math.min(e/g.width,k/g.height);if(h<1){e=Math.round(g.width*h);k=Math.round(g.height*h)}else{e=g.width;k=g.height}g.resize(e,k);return g.encode(d,{quality:j/100})}b.runtimes.Gears=b.addRuntime("gears",{getFeatures:function(){return{dragdrop:true,jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,i){var h;if(!window.google||!google.gears){return i({success:false})}try{h=google.gears.factory.create("beta.desktop")}catch(f){return i({success:false})}function d(k){var j,e,l=[],m;for(e=0;e<k.length;e++){j=k[e];m=b.guid();c[m]=j.blob;l.push(new b.File(m,j.name,j.blob.length))}g.trigger("FilesAdded",l)}g.bind("PostInit",function(){var j=g.settings,e=document.getElementById(j.drop_element);if(e){b.addEvent(e,"dragover",function(k){h.setDropEffect(k,"copy");k.preventDefault()});b.addEvent(e,"drop",function(l){var k=h.getDragData(l,"application/x-gears-files");if(k){d(k.files)}l.preventDefault()});e=0}b.addEvent(document.getElementById(j.browse_button),"click",function(o){var n=[],l,k,m;o.preventDefault();for(l=0;l<j.filters.length;l++){m=j.filters[l].extensions.split(",");for(k=0;k<m.length;k++){n.push("."+m[k])}}h.openFiles(d,{singleFile:!j.multi_selection,filter:n})})});g.bind("UploadFile",function(o,l){var q=0,p,m,n=0,k=o.settings.resize,e;m=o.settings.chunk_size;e=m>0;p=Math.ceil(l.size/m);if(!e){m=l.size;p=1}if(k&&/\.(png|jpg|jpeg)$/i.test(l.name)){c[l.id]=a(c[l.id],k.width,k.height,k.quality||90,/\.png$/i.test(l.name)?"image/png":"image/jpeg")}l.size=c[l.id].length;function j(){var u,w,s=o.settings.multipart,r=0,v={name:l.target_name||l.name};function t(y){var x,C="----pluploadboundary"+b.guid(),A="--",B="\r\n",z;if(s){u.setRequestHeader("Content-Type","multipart/form-data; boundary="+C);x=google.gears.factory.create("beta.blobbuilder");b.each(o.settings.multipart_params,function(E,D){x.append(A+C+B+'Content-Disposition: form-data; name="'+D+'"'+B+B);x.append(E+B)});x.append(A+C+B+'Content-Disposition: form-data; name="'+o.settings.file_data_name+'"; filename="'+l.name+'"'+B+"Content-Type: application/octet-stream"+B+B);x.append(y);x.append(B+A+C+A+B);z=x.getAsBlob();r=z.length-y.length;y=z}u.send(y)}if(l.status==b.DONE||l.status==b.FAILED||o.state==b.STOPPED){return}if(e){v.chunk=q;v.chunks=p}w=Math.min(m,l.size-(q*m));u=google.gears.factory.create("beta.httprequest");u.open("POST",b.buildUrl(o.settings.url,v));if(!s){u.setRequestHeader("Content-Disposition",'attachment; filename="'+l.name+'"');u.setRequestHeader("Content-Type","application/octet-stream")}b.each(o.settings.headers,function(y,x){u.setRequestHeader(x,y)});u.upload.onprogress=function(x){l.loaded=n+x.loaded-r;o.trigger("UploadProgress",l)};u.onreadystatechange=function(){var x;if(u.readyState==4){if(u.status==200){x={chunk:q,chunks:p,response:u.responseText,status:u.status};o.trigger("ChunkUploaded",l,x);if(x.cancelled){l.status=b.FAILED;return}n+=w;if(++q>=p){l.status=b.DONE;o.trigger("FileUploaded",l,{response:u.responseText,status:u.status})}else{j()}}else{o.trigger("Error",{code:b.HTTP_ERROR,message:"HTTP Error.",file:l,chunk:q,chunks:p,status:u.status})}}};if(q<p){t(c[l.id].slice(q*m,w))}}j()});i({success:true})}})})(plupload);(function(c){var a={};function b(l){var k,j=typeof l,h,e,g,f;if(j==="string"){k="\bb\tt\nn\ff\rr\"\"''\\\\";return'"'+l.replace(/([\u0080-\uFFFF\x00-\x1f\"])/g,function(n,m){var i=k.indexOf(m);if(i+1){return"\\"+k.charAt(i+1)}n=m.charCodeAt().toString(16);return"\\u"+"0000".substring(n.length)+n})+'"'}if(j=="object"){e=l.length!==h;k="";if(e){for(g=0;g<l.length;g++){if(k){k+=","}k+=b(l[g])}k="["+k+"]"}else{for(f in l){if(l.hasOwnProperty(f)){if(k){k+=","}k+=b(f)+":"+b(l[f])}}k="{"+k+"}"}return k}if(l===h){return"null"}return""+l}function d(o){var r=false,f=null,k=null,g,h,i,q,j,m=0;try{try{k=new ActiveXObject("AgControl.AgControl");if(k.IsVersionSupported(o)){r=true}k=null}catch(n){var l=navigator.plugins["Silverlight Plug-In"];if(l){g=l.description;if(g==="1.0.30226.2"){g="2.0.30226.2"}h=g.split(".");while(h.length>3){h.pop()}while(h.length<4){h.push(0)}i=o.split(".");while(i.length>4){i.pop()}do{q=parseInt(i[m],10);j=parseInt(h[m],10);m++}while(m<i.length&&q===j);if(q<=j&&!isNaN(q)){r=true}}}}catch(p){r=false}return r}c.silverlight={trigger:function(j,f){var h=a[j],g,e;if(h){e=c.toArray(arguments).slice(1);e[0]="Silverlight:"+f;setTimeout(function(){h.trigger.apply(h,e)},0)}}};c.runtimes.Silverlight=c.addRuntime("silverlight",{getFeatures:function(){return{jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(l,m){var k,h="",j=l.settings.filters,g,f=document.body;if(!d("2.0.31005.0")||(window.opera&&window.opera.buildNumber)){m({success:false});return}a[l.id]=l;k=document.createElement("div");k.id=l.id+"_silverlight_container";c.extend(k.style,{position:"absolute",top:"0px",background:l.settings.shim_bgcolor||"transparent",zIndex:99999,width:"100px",height:"100px",overflow:"hidden",opacity:l.settings.shim_bgcolor?"":0.01});k.className="plupload silverlight";if(l.settings.container){f=document.getElementById(l.settings.container);f.style.position="relative"}f.appendChild(k);for(g=0;g<j.length;g++){h+=(h!=""?"|":"")+j[g].title+" | *."+j[g].extensions.replace(/,/g,";*.")}k.innerHTML='<object id="'+l.id+'_silverlight" data="data:application/x-silverlight," type="application/x-silverlight-2" style="outline:none;" width="1024" height="1024"><param name="source" value="'+l.settings.silverlight_xap_url+'"/><param name="background" value="Transparent"/><param name="windowless" value="true"/><param name="initParams" value="id='+l.id+",filter="+h+'"/></object>';function e(){return document.getElementById(l.id+"_silverlight").content.Upload}l.bind("Silverlight:Init",function(){var i,n={};l.bind("Silverlight:StartSelectFiles",function(o){i=[]});l.bind("Silverlight:SelectFile",function(o,r,p,q){var s;s=c.guid();n[s]=r;n[r]=s;i.push(new c.File(s,p,q))});l.bind("Silverlight:SelectSuccessful",function(){if(i.length){l.trigger("FilesAdded",i)}});l.bind("Silverlight:UploadChunkError",function(o,r,p,s,q){l.trigger("Error",{code:c.IO_ERROR,message:"IO Error.",details:q,file:o.getFile(n[r])})});l.bind("Silverlight:UploadFileProgress",function(o,s,p,r){var q=o.getFile(n[s]);if(q.status!=c.FAILED){q.size=r;q.loaded=p;o.trigger("UploadProgress",q)}});l.bind("Refresh",function(o){var p,q,r;p=document.getElementById(o.settings.browse_button);q=c.getPos(p,document.getElementById(o.settings.container));r=c.getSize(p);c.extend(document.getElementById(o.id+"_silverlight_container").style,{top:q.y+"px",left:q.x+"px",width:r.w+"px",height:r.h+"px"})});l.bind("Silverlight:UploadChunkSuccessful",function(o,r,p,u,t){var s,q=o.getFile(n[r]);s={chunk:p,chunks:u,response:t};o.trigger("ChunkUploaded",q,s);if(q.status!=c.FAILED){e().UploadNextChunk()}if(p==u-1){q.status=c.DONE;o.trigger("FileUploaded",q,{response:t})}});l.bind("Silverlight:UploadSuccessful",function(o,r,p){var q=o.getFile(n[r]);q.status=c.DONE;o.trigger("FileUploaded",q,{response:p})});l.bind("FilesRemoved",function(o,q){var p;for(p=0;p<q.length;p++){e().RemoveFile(n[q[p].id])}});l.bind("UploadFile",function(o,q){var r=o.settings,p=r.resize||{};e().UploadFile(n[q.id],c.buildUrl(o.settings.url,{name:q.target_name||q.name}),b({chunk_size:r.chunk_size,image_width:p.width,image_height:p.height,image_quality:p.quality||90,multipart:!!r.multipart,multipart_params:r.multipart_params||{},headers:r.headers}))});m({success:true})})}})})(plupload);(function(c){var a={};function b(){var d;try{d=navigator.plugins["Shockwave Flash"];d=d.description}catch(f){try{d=new ActiveXObject("ShockwaveFlash.ShockwaveFlash").GetVariable("$version")}catch(e){d="0.0"}}d=d.match(/\d+/g);return parseFloat(d[0]+"."+d[1])}c.flash={trigger:function(f,d,e){setTimeout(function(){var j=a[f],h,g;if(j){j.trigger("Flash:"+d,e)}},0)}};c.runtimes.Flash=c.addRuntime("flash",{getFeatures:function(){return{jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,l){var k,f,h,e,m=0,d=document.body;if(b()<10){l({success:false});return}a[g.id]=g;k=document.getElementById(g.settings.browse_button);f=document.createElement("div");f.id=g.id+"_flash_container";c.extend(f.style,{position:"absolute",top:"0px",background:g.settings.shim_bgcolor||"transparent",zIndex:99999,width:"100%",height:"100%"});f.className="plupload flash";if(g.settings.container){d=document.getElementById(g.settings.container);d.style.position="relative"}d.appendChild(f);h="id="+escape(g.id);f.innerHTML='<object id="'+g.id+'_flash" width="100%" height="100%" style="outline:0" type="application/x-shockwave-flash" data="'+g.settings.flash_swf_url+'"><param name="movie" value="'+g.settings.flash_swf_url+'" /><param name="flashvars" value="'+h+'" /><param name="wmode" value="transparent" /><param name="allowscriptaccess" value="always" /></object>';function j(){return document.getElementById(g.id+"_flash")}function i(){if(m++>5000){l({success:false});return}if(!e){setTimeout(i,1)}}i();k=f=null;g.bind("Flash:Init",function(){var p={},o,n=g.settings.resize||{};e=true;j().setFileFilters(g.settings.filters,g.settings.multi_selection);g.bind("UploadFile",function(q,r){var s=q.settings;j().uploadFile(p[r.id],c.buildUrl(s.url,{name:r.target_name||r.name}),{chunk_size:s.chunk_size,width:n.width,height:n.height,quality:n.quality||90,multipart:s.multipart,multipart_params:s.multipart_params,file_data_name:s.file_data_name,format:/\.(jpg|jpeg)$/i.test(r.name)?"jpg":"png",headers:s.headers})});g.bind("Flash:UploadProcess",function(r,q){var s=r.getFile(p[q.id]);if(s.status!=c.FAILED){s.loaded=q.loaded;s.size=q.size;r.trigger("UploadProgress",s)}});g.bind("Flash:UploadChunkComplete",function(q,s){var t,r=q.getFile(p[s.id]);t={chunk:s.chunk,chunks:s.chunks,response:s.text};q.trigger("ChunkUploaded",r,t);if(r.status!=c.FAILED){j().uploadNextChunk()}if(s.chunk==s.chunks-1){r.status=c.DONE;q.trigger("FileUploaded",r,{response:s.text})}});g.bind("Flash:SelectFiles",function(q,t){var s,r,u=[],v;for(r=0;r<t.length;r++){s=t[r];v=c.guid();p[v]=s.id;p[s.id]=v;u.push(new c.File(v,s.name,s.size))}if(u.length){g.trigger("FilesAdded",u)}});g.bind("Flash:SecurityError",function(q,r){g.trigger("Error",{code:c.SECURITY_ERROR,message:"Security error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("Flash:GenericError",function(q,r){g.trigger("Error",{code:c.GENERIC_ERROR,message:"Generic error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("Flash:IOError",function(q,r){g.trigger("Error",{code:c.IO_ERROR,message:"IO error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("QueueChanged",function(q){g.refresh()});g.bind("FilesRemoved",function(q,s){var r;for(r=0;r<s.length;r++){j().removeFile(p[s[r].id])}});g.bind("StateChanged",function(q){g.refresh()});g.bind("Refresh",function(q){var r,s,t;j().setFileFilters(g.settings.filters,g.settings.multi_selection);r=document.getElementById(q.settings.browse_button);s=c.getPos(r,document.getElementById(q.settings.container));t=c.getSize(r);c.extend(document.getElementById(q.id+"_flash_container").style,{top:s.y+"px",left:s.x+"px",width:t.w+"px",height:t.h+"px"})});l({success:true})})}})})(plupload);(function(a){a.runtimes.BrowserPlus=a.addRuntime("browserplus",{getFeatures:function(){return{dragdrop:true,jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,i){var e=window.BrowserPlus,h={},d=g.settings,c=d.resize;function f(n){var m,l,j=[],k,o;for(l=0;l<n.length;l++){k=n[l];o=a.guid();h[o]=k;j.push(new a.File(o,k.name,k.size))}if(l){g.trigger("FilesAdded",j)}}function b(){g.bind("PostInit",function(){var m,k=d.drop_element,o=g.id+"_droptarget",j=document.getElementById(k),l;function p(r,q){e.DragAndDrop.AddDropTarget({id:r},function(s){e.DragAndDrop.AttachCallbacks({id:r,hover:function(t){if(!t&&q){q()}},drop:function(t){if(q){q()}f(t)}},function(){})})}function n(){document.getElementById(o).style.top="-1000px"}if(j){if(document.attachEvent&&(/MSIE/gi).test(navigator.userAgent)){m=document.createElement("div");m.setAttribute("id",o);a.extend(m.style,{position:"absolute",top:"-1000px",background:"red",filter:"alpha(opacity=0)",opacity:0});document.body.appendChild(m);a.addEvent(j,"dragenter",function(r){var q,s;q=document.getElementById(k);s=a.getPos(q);a.extend(document.getElementById(o).style,{top:s.y+"px",left:s.x+"px",width:q.offsetWidth+"px",height:q.offsetHeight+"px"})});p(o,n)}else{p(k)}}a.addEvent(document.getElementById(d.browse_button),"click",function(v){var t=[],r,q,u=d.filters,s;v.preventDefault();for(r=0;r<u.length;r++){s=u[r].extensions.split(",");for(q=0;q<s.length;q++){t.push(a.mimeTypes[s[q]])}}e.FileBrowse.OpenBrowseDialog({mimeTypes:t},function(w){if(w.success){f(w.value)}})});j=m=null});g.bind("UploadFile",function(n,k){var m=h[k.id],j={},l=n.settings.chunk_size,o,p=[];function r(s,u){var t;if(k.status==a.FAILED){return}j.name=k.target_name||k.name;if(l){j.chunk=s;j.chunks=u}t=p.shift();e.Uploader.upload({url:a.buildUrl(n.settings.url,j),files:{file:t},cookies:document.cookies,postvars:n.settings.multipart_params,progressCallback:function(x){var w,v=0;o[s]=parseInt(x.filePercent*t.size/100,10);for(w=0;w<o.length;w++){v+=o[w]}k.loaded=v;n.trigger("UploadProgress",k)}},function(w){var v,x;if(w.success){v=w.value.statusCode;if(l){n.trigger("ChunkUploaded",k,{chunk:s,chunks:u,response:w.value.body,status:v})}if(p.length>0){r(++s,u)}else{k.status=a.DONE;n.trigger("FileUploaded",k,{response:w.value.body,status:v});if(v>=400){n.trigger("Error",{code:a.HTTP_ERROR,message:"HTTP Error.",file:k,status:v})}}}else{n.trigger("Error",{code:a.GENERIC_ERROR,message:"Generic Error.",file:k,details:w.error})}})}function q(s){k.size=s.size;if(l){e.FileAccess.chunk({file:s,chunkSize:l},function(v){if(v.success){var w=v.value,t=w.length;o=Array(t);for(var u=0;u<t;u++){o[u]=0;p.push(w[u])}r(0,t)}})}else{o=Array(1);p.push(s);r(0,1)}}if(c&&/\.(png|jpg|jpeg)$/i.test(k.name)){BrowserPlus.ImageAlter.transform({file:m,quality:c.quality||90,actions:[{scale:{maxwidth:c.width,maxheight:c.height}}]},function(s){if(s.success){q(s.value.file)}})}else{q(m)}});i({success:true})}if(e){e.init(function(k){var j=[{service:"Uploader",version:"3"},{service:"DragAndDrop",version:"1"},{service:"FileBrowse",version:"1"},{service:"FileAccess",version:"2"}];if(c){j.push({service:"ImageAlter",version:"4"})}if(k.success){e.require({services:j},function(l){if(l.success){b()}else{i()}})}else{i()}})}else{i()}}})})(plupload);(function(b){function a(i,l,j,c,k){var e,d,h,g,f;e=document.createElement("canvas");e.style.display="none";document.body.appendChild(e);d=e.getContext("2d");h=new Image();h.onload=function(){var o,m,n;f=Math.min(l/h.width,j/h.height);if(f<1){o=Math.round(h.width*f);m=Math.round(h.height*f)}else{o=h.width;m=h.height}e.width=o;e.height=m;d.drawImage(h,0,0,o,m);g=e.toDataURL(c);g=g.substring(g.indexOf("base64,")+7);g=atob(g);e.parentNode.removeChild(e);k({success:true,data:g})};h.src=i}b.runtimes.Html5=b.addRuntime("html5",{getFeatures:function(){var g,d,f,e,c;d=f=e=c=false;if(window.XMLHttpRequest){g=new XMLHttpRequest();f=!!g.upload;d=!!(g.sendAsBinary||g.upload)}if(d){e=!!(File&&File.prototype.getAsDataURL);c=!!(File&&File.prototype.slice)}return{html5:d,dragdrop:window.mozInnerScreenX!==undefined||c,jpgresize:e,pngresize:e,multipart:e,progress:f}},init:function(e,f){var c={};function d(k){var h,g,j=[],l;for(g=0;g<k.length;g++){h=k[g];l=b.guid();c[l]=h;j.push(new b.File(l,h.fileName,h.fileSize))}if(j.length){e.trigger("FilesAdded",j)}}if(!this.getFeatures().html5){f({success:false});return}e.bind("Init",function(l){var p,n=[],k,o,h=l.settings.filters,j,m,g=document.body;p=document.createElement("div");p.id=l.id+"_html5_container";for(k=0;k<h.length;k++){j=h[k].extensions.split(/,/);for(o=0;o<j.length;o++){m=b.mimeTypes[j[o]];if(m){n.push(m)}}}b.extend(p.style,{position:"absolute",background:e.settings.shim_bgcolor||"transparent",width:"100px",height:"100px",overflow:"hidden",zIndex:99999,opacity:e.settings.shim_bgcolor?"":0});p.className="plupload html5";if(e.settings.container){g=document.getElementById(e.settings.container);g.style.position="relative"}g.appendChild(p);p.innerHTML='<input id="'+e.id+'_html5" style="width:100%;" type="file" accept="'+n.join(",")+'" '+(e.settings.multi_selection?'multiple="multiple"':"")+" />";document.getElementById(e.id+"_html5").onchange=function(){d(this.files);this.value=""}});e.bind("PostInit",function(){var g=document.getElementById(e.settings.drop_element);if(g){b.addEvent(g,"dragover",function(h){h.preventDefault()});b.addEvent(g,"drop",function(i){var h=i.dataTransfer;if(h&&h.files){d(h.files)}i.preventDefault()})}});e.bind("Refresh",function(g){var h,i,j;h=document.getElementById(e.settings.browse_button);i=b.getPos(h,document.getElementById(g.settings.container));j=b.getSize(h);b.extend(document.getElementById(e.id+"_html5_container").style,{top:i.y+"px",left:i.x+"px",width:j.w+"px",height:j.h+"px"})});e.bind("UploadFile",function(g,j){var n=new XMLHttpRequest(),i=n.upload,h=g.settings.resize,m,l=0;function k(o){var s="----pluploadboundary"+b.guid(),q="--",r="\r\n",p="";if(g.settings.multipart){n.setRequestHeader("Content-Type","multipart/form-data; boundary="+s);b.each(g.settings.multipart_params,function(u,t){p+=q+s+r+'Content-Disposition: form-data; name="'+t+'"'+r+r;p+=u+r});p+=q+s+r+'Content-Disposition: form-data; name="'+g.settings.file_data_name+'"; filename="'+j.name+'"'+r+"Content-Type: application/octet-stream"+r+r+o+r+q+s+q+r;l=p.length-o.length;o=p}n.sendAsBinary(o)}if(j.status==b.DONE||j.status==b.FAILED||g.state==b.STOPPED){return}if(i){i.onprogress=function(o){j.loaded=o.loaded-l;g.trigger("UploadProgress",j)}}n.onreadystatechange=function(){var o;if(n.readyState==4){try{o=n.status}catch(p){o=0}j.status=b.DONE;j.loaded=j.size;g.trigger("UploadProgress",j);g.trigger("FileUploaded",j,{response:n.responseText,status:o});if(o>=400){g.trigger("Error",{code:b.HTTP_ERROR,message:"HTTP Error.",file:j,status:o})}}};n.open("post",b.buildUrl(g.settings.url,{name:j.target_name||j.name}),true);n.setRequestHeader("Content-Type","application/octet-stream");b.each(g.settings.headers,function(p,o){n.setRequestHeader(o,p)});m=c[j.id];if(n.sendAsBinary){if(h&&/\.(png|jpg|jpeg)$/i.test(j.name)){a(m.getAsDataURL(),h.width,h.height,/\.png$/i.test(j.name)?"image/png":"image/jpeg",function(o){if(o.success){j.size=o.data.length;k(o.data)}else{k(m.getAsBinary())}})}else{k(m.getAsBinary())}}else{n.send(m)}});f({success:true})}})})(plupload);(function(a){a.runtimes.Html4=a.addRuntime("html4",{getFeatures:function(){return{multipart:true}},init:function(f,g){var d={},c,b;function e(l){var k,j,m=[],n,h;h=l.value.replace(/\\/g,"/");h=h.substring(h.length,h.lastIndexOf("/")+1);n=a.guid();k=new a.File(n,h);d[n]=k;k.input=l;m.push(k);if(m.length){f.trigger("FilesAdded",m)}}f.bind("Init",function(p){var h,x,v,t=[],o,u,m=p.settings.filters,l,s,r=/MSIE/.test(navigator.userAgent),k="javascript",w,j=document.body,n;if(f.settings.container){j=document.getElementById(f.settings.container);j.style.position="relative"}c=(typeof p.settings.form=="string")?document.getElementById(p.settings.form):p.settings.form;if(!c){n=document.getElementById(f.settings.browse_button);for(;n;n=n.parentNode){if(n.nodeName=="FORM"){c=n}}}if(!c){c=document.createElement("form");c.style.display="inline";n=document.getElementById(f.settings.container);n.parentNode.insertBefore(c,n);c.appendChild(n)}c.setAttribute("method","post");c.setAttribute("enctype","multipart/form-data");a.each(p.settings.multipart_params,function(z,y){var i=document.createElement("input");a.extend(i,{type:"hidden",name:y,value:z});c.appendChild(i)});b=document.createElement("iframe");b.setAttribute("src",k+':""');b.setAttribute("name",p.id+"_iframe");b.setAttribute("id",p.id+"_iframe");b.style.display="none";a.addEvent(b,"load",function(B){var C=B.target,z=f.currentfile,A;try{A=C.contentWindow.document||C.contentDocument||window.frames[C.id].document}catch(y){p.trigger("Error",{code:a.SECURITY_ERROR,message:"Security error.",file:z});return}if(A.location.href=="about:blank"||!z){return}var i=A.documentElement.innerText||A.documentElement.textContent;if(i!=""){z.status=a.DONE;z.loaded=1025;z.percent=100;if(z.input){z.input.removeAttribute("name")}p.trigger("UploadProgress",z);p.trigger("FileUploaded",z,{response:i});if(c.tmpAction){c.setAttribute("action",c.tmpAction)}if(c.tmpTarget){c.setAttribute("target",c.tmpTarget)}}});c.appendChild(b);if(r){window.frames[b.id].name=b.name}x=document.createElement("div");x.id=p.id+"_iframe_container";for(o=0;o<m.length;o++){l=m[o].extensions.split(/,/);for(u=0;u<l.length;u++){s=a.mimeTypes[l[u]];if(s){t.push(s)}}}a.extend(x.style,{position:"absolute",background:"transparent",width:"100px",height:"100px",overflow:"hidden",zIndex:99999,opacity:0});w=f.settings.shim_bgcolor;if(w){a.extend(x.style,{background:w,opacity:1})}x.className="plupload_iframe";j.appendChild(x);function q(){v=document.createElement("input");v.setAttribute("type","file");v.setAttribute("accept",t.join(","));v.setAttribute("size",1);a.extend(v.style,{width:"100%",height:"100%",opacity:0});if(r){a.extend(v.style,{filter:"alpha(opacity=0)"})}a.addEvent(v,"change",function(i){var y=i.target;if(y.value){q();y.style.display="none";e(y)}});x.appendChild(v);return true}q()});f.bind("Refresh",function(h){var i,j,k;i=document.getElementById(f.settings.browse_button);j=a.getPos(i,document.getElementById(h.settings.container));k=a.getSize(i);a.extend(document.getElementById(f.id+"_iframe_container").style,{top:j.y+"px",left:j.x+"px",width:k.w+"px",height:k.h+"px"})});f.bind("UploadFile",function(h,i){if(i.status==a.DONE||i.status==a.FAILED||h.state==a.STOPPED){return}if(!i.input){i.status=a.ERROR;return}i.input.setAttribute("name",h.settings.file_data_name);c.tmpAction=c.getAttribute("action");c.setAttribute("action",a.buildUrl(h.settings.url,{name:i.target_name||i.name}));c.tmpTarget=c.getAttribute("target");c.setAttribute("target",b.name);this.currentfile=i;c.submit()});f.bind("FilesRemoved",function(h,k){var j,l;for(j=0;j<k.length;j++){l=k[j].input;if(l){l.parentNode.removeChild(l)}}});g({success:true})}})})(plupload);
(function(c){var d={};function a(e){return plupload.translate(e)||e}function b(f,e){e.contents().each(function(g,h){h=c(h);if(!h.is(".plupload")){h.remove()}});e.prepend('<div class="plupload_wrapper plupload_scroll"><div id="'+f+'_container" class="plupload_container"><div class="plupload"><div class="plupload_header"><div class="plupload_header_content"><div class="plupload_header_title">'+a("Select files")+'</div><div class="plupload_header_text">'+a("Add files to the upload queue and click the start button.")+'</div></div></div><div class="plupload_content"><div class="plupload_filelist_header"><div class="plupload_file_name">'+a("Filename")+'</div><div class="plupload_file_action">&nbsp;</div><div class="plupload_file_status"><span>'+a("Status")+'</span></div><div class="plupload_file_size">'+a("Size")+'</div><div class="plupload_clearer">&nbsp;</div></div><ul id="'+f+'_filelist" class="plupload_filelist"></ul><div class="plupload_filelist_footer"><div class="plupload_file_name"><div class="plupload_buttons"><a href="#" class="plupload_button plupload_add">'+a("Add files")+'</a><a href="#" class="plupload_button plupload_start">'+a("Start upload")+'</a></div><span class="plupload_upload_status"></span></div><div class="plupload_file_action"></div><div class="plupload_file_status"><span class="plupload_total_status">0%</span></div><div class="plupload_file_size"><span class="plupload_total_file_size">0 b</span></div><div class="plupload_progress"><div class="plupload_progress_container"><div class="plupload_progress_bar"></div></div></div><div class="plupload_clearer">&nbsp;</div></div></div></div></div><input type="hidden" id="'+f+'_count" name="'+f+'_count" value="0" /></div>')}c.fn.pluploadQueue=function(e){if(e){this.each(function(){var j,i,k;i=c(this);k=i.attr("id");if(!k){k=plupload.guid();i.attr("id",k)}j=new plupload.Uploader(c.extend({dragdrop:true,container:k},e));if(e.preinit){e.preinit(j)}d[k]=j;function h(l){var m;if(l.status==plupload.DONE){m="plupload_done"}if(l.status==plupload.FAILED){m="plupload_failed"}if(l.status==plupload.QUEUED){m="plupload_delete"}if(l.status==plupload.UPLOADING){m="plupload_uploading"}c("#"+l.id).attr("class",m).find("a").css("display","block")}function f(){c("span.plupload_total_status",i).html(j.total.percent+"%");c("div.plupload_progress_bar",i).css("width",j.total.percent+"%");c("span.plupload_upload_status",i).text("Uploaded "+j.total.uploaded+"/"+j.files.length+" files");if(j.total.uploaded==j.files.length){j.stop()}}function g(){var m=c("ul.plupload_filelist",i).html(""),n=0,l;c.each(j.files,function(p,o){l="";if(o.status==plupload.DONE){if(o.target_name){l+='<input type="hidden" name="'+k+"_"+n+'_tmpname" value="'+plupload.xmlEncode(o.target_name)+'" />'}l+='<input type="hidden" name="'+k+"_"+n+'_name" value="'+plupload.xmlEncode(o.name)+'" />';l+='<input type="hidden" name="'+k+"_"+n+'_status" value="'+(o.status==plupload.DONE?"done":"failed")+'" />';n++;c("#"+k+"_count").val(n)}m.append('<li id="'+o.id+'"><div class="plupload_file_name"><span>'+o.name+'</span></div><div class="plupload_file_action"><a href="#"></a></div><div class="plupload_file_status">'+o.percent+'%</div><div class="plupload_file_size">'+plupload.formatSize(o.size)+'</div><div class="plupload_clearer">&nbsp;</div>'+l+"</li>");h(o);c("#"+o.id+".plupload_delete a").click(function(q){c("#"+o.id).remove();j.removeFile(o);q.preventDefault()})});c("span.plupload_total_file_size",i).html(plupload.formatSize(j.total.size));if(j.total.queued===0){c("span.plupload_add_text",i).text(a("Add files."))}else{c("span.plupload_add_text",i).text(j.total.queued+" files queued.")}c("a.plupload_start",i).toggleClass("plupload_disabled",j.files.length===0);m[0].scrollTop=m[0].scrollHeight;f();if(!j.files.length&&j.features.dragdrop&&j.settings.dragdrop){c("#"+k+"_filelist").append('<li class="plupload_droptext">'+a("Drag files here.")+"</li>")}}j.bind("UploadFile",function(l,m){c("#"+m.id).addClass("plupload_current_file")});j.bind("Init",function(l,m){b(k,i);if(!e.unique_names&&e.rename){c("#"+k+"_filelist div.plupload_file_name span",i).live("click",function(s){var q=c(s.target),o,r,n,p="";o=l.getFile(q.parents("li")[0].id);n=o.name;r=/^(.+)(\.[^.]+)$/.exec(n);if(r){n=r[1];p=r[2]}q.hide().after('<input type="text" />');q.next().val(n).focus().blur(function(){q.show().next().remove()}).keydown(function(u){var t=c(this);if(u.keyCode==13){u.preventDefault();o.name=t.val()+p;q.text(o.name);t.blur()}})})}c("a.plupload_add",i).attr("id",k+"_browse");l.settings.browse_button=k+"_browse";if(l.features.dragdrop&&l.settings.dragdrop){l.settings.drop_element=k+"_filelist";c("#"+k+"_filelist").append('<li class="plupload_droptext">'+a("Drag files here.")+"</li>")}c("#"+k+"_container").attr("title","Using runtime: "+m.runtime);c("a.plupload_start",i).click(function(n){if(!c(this).hasClass("plupload_disabled")){j.start()}n.preventDefault()});c("a.plupload_stop",i).click(function(n){j.stop();n.preventDefault()});c("a.plupload_start",i).addClass("plupload_disabled")});j.init();if(e.setup){e.setup(j)}j.bind("Error",function(l,o){var m=o.file,n;if(m){n=o.message;if(o.details){n+=" ("+o.details+")"}c("#"+m.id).attr("class","plupload_failed").find("a").css("display","block").attr("title",n)}});j.bind("StateChanged",function(){if(j.state===plupload.STARTED){c("li.plupload_delete a,div.plupload_buttons",i).hide();c("span.plupload_upload_status,div.plupload_progress,a.plupload_stop",i).css("display","block");c("span.plupload_upload_status",i).text("Uploaded 0/"+j.files.length+" files")}else{c("a.plupload_stop,div.plupload_progress",i).hide();c("a.plupload_delete",i).css("display","block")}});j.bind("QueueChanged",g);j.bind("StateChanged",function(l){if(l.state==plupload.STOPPED){g()}});j.bind("FileUploaded",function(l,m){h(m)});j.bind("UploadProgress",function(l,m){c("#"+m.id+" div.plupload_file_status",i).html(m.percent+"%");h(m);f()})});return this}else{return d[c(this[0]).attr("id")]}}})(jQuery);
 /*
 * TipTip
 * Copyright 2010 Drew Wilson
 * www.drewwilson.com
 * code.drewwilson.com/entry/tiptip-jquery-plugin
 *
 * Version 1.3   -   Updated: Mar. 23, 2010
 *
 * This Plug-In will create a custom tooltip to replace the default
 * browser tooltip. It is extremely lightweight and very smart in
 * that it detects the edges of the browser window and will make sure
 * the tooltip stays within the current window size. As a result the
 * tooltip will adjust itself to be displayed above, below, to the left 
 * or to the right depending on what is necessary to stay within the
 * browser window. It is completely customizable as well via CSS.
 *
 * This TipTip jQuery plug-in is dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
(function($){$.fn.tipTip=function(options){var defaults={activation:"hover",keepAlive:false,maxWidth:"200px",edgeOffset:3,defaultPosition:"bottom",delay:400,fadeIn:200,fadeOut:200,attribute:"title",content:false,enter:function(){},exit:function(){}};var opts=$.extend(defaults,options);if($("#tiptip_holder").length<=0){var tiptip_holder=$('<div id="tiptip_holder" style="max-width:'+opts.maxWidth+';"></div>');var tiptip_content=$('<div id="tiptip_content"></div>');var tiptip_arrow=$('<div id="tiptip_arrow"></div>');$("body").append(tiptip_holder.html(tiptip_content).prepend(tiptip_arrow.html('<div id="tiptip_arrow_inner"></div>')))}else{var tiptip_holder=$("#tiptip_holder");var tiptip_content=$("#tiptip_content");var tiptip_arrow=$("#tiptip_arrow")}return this.each(function(){var org_elem=$(this);if(opts.content){var org_title=opts.content}else{var org_title=org_elem.attr(opts.attribute)}if(org_title!=""){if(!opts.content){org_elem.removeAttr(opts.attribute)}var timeout=false;if(opts.activation=="hover"){org_elem.hover(function(){active_tiptip()},function(){if(!opts.keepAlive){deactive_tiptip()}});if(opts.keepAlive){tiptip_holder.hover(function(){},function(){deactive_tiptip()})}}else if(opts.activation=="focus"){org_elem.focus(function(){active_tiptip()}).blur(function(){deactive_tiptip()})}else if(opts.activation=="click"){org_elem.click(function(){active_tiptip();return false}).hover(function(){},function(){if(!opts.keepAlive){deactive_tiptip()}});if(opts.keepAlive){tiptip_holder.hover(function(){},function(){deactive_tiptip()})}}function active_tiptip(){opts.enter.call(this);tiptip_content.html(org_title);tiptip_holder.hide().removeAttr("class").css("margin","0");tiptip_arrow.removeAttr("style");var top=parseInt(org_elem.offset()['top']);var left=parseInt(org_elem.offset()['left']);var org_width=parseInt(org_elem.outerWidth());var org_height=parseInt(org_elem.outerHeight());var tip_w=tiptip_holder.outerWidth();var tip_h=tiptip_holder.outerHeight();var w_compare=Math.round((org_width-tip_w)/2);var h_compare=Math.round((org_height-tip_h)/2);var marg_left=Math.round(left+w_compare);var marg_top=Math.round(top+org_height+opts.edgeOffset);var t_class="";var arrow_top="";var arrow_left=Math.round(tip_w-12)/2;if(opts.defaultPosition=="bottom"){t_class="_bottom"}else if(opts.defaultPosition=="top"){t_class="_top"}else if(opts.defaultPosition=="left"){t_class="_left"}else if(opts.defaultPosition=="right"){t_class="_right"}var right_compare=(w_compare+left)<parseInt($(window).scrollLeft());var left_compare=(tip_w+left)>parseInt($(window).width());if((right_compare&&w_compare<0)||(t_class=="_right"&&!left_compare)||(t_class=="_left"&&left<(tip_w+opts.edgeOffset+5))){t_class="_right";arrow_top=Math.round(tip_h-13)/2;arrow_left=-12;marg_left=Math.round(left+org_width+opts.edgeOffset);marg_top=Math.round(top+h_compare)}else if((left_compare&&w_compare<0)||(t_class=="_left"&&!right_compare)){t_class="_left";arrow_top=Math.round(tip_h-13)/2;arrow_left=Math.round(tip_w);marg_left=Math.round(left-(tip_w+opts.edgeOffset+5));marg_top=Math.round(top+h_compare)}var top_compare=(top+org_height+opts.edgeOffset+tip_h+8)>parseInt($(window).height()+$(window).scrollTop());var bottom_compare=((top+org_height)-(opts.edgeOffset+tip_h+8))<0;if(top_compare||(t_class=="_bottom"&&top_compare)||(t_class=="_top"&&!bottom_compare)){if(t_class=="_top"||t_class=="_bottom"){t_class="_top"}else{t_class=t_class+"_top"}arrow_top=tip_h;marg_top=Math.round(top-(tip_h+5+opts.edgeOffset))}else if(bottom_compare|(t_class=="_top"&&bottom_compare)||(t_class=="_bottom"&&!top_compare)){if(t_class=="_top"||t_class=="_bottom"){t_class="_bottom"}else{t_class=t_class+"_bottom"}arrow_top=-12;marg_top=Math.round(top+org_height+opts.edgeOffset)}if(t_class=="_right_top"||t_class=="_left_top"){marg_top=marg_top+5}else if(t_class=="_right_bottom"||t_class=="_left_bottom"){marg_top=marg_top-5}if(t_class=="_left_top"||t_class=="_left_bottom"){marg_left=marg_left+5}tiptip_arrow.css({"margin-left":arrow_left+"px","margin-top":arrow_top+"px"});tiptip_holder.css({"margin-left":marg_left+"px","margin-top":marg_top+"px"}).attr("class","tip"+t_class);if(timeout){clearTimeout(timeout)}timeout=setTimeout(function(){tiptip_holder.stop(true,true).fadeIn(opts.fadeIn)},opts.delay)}function deactive_tiptip(){opts.exit.call(this);if(timeout){clearTimeout(timeout)}tiptip_holder.fadeOut(opts.fadeOut)}}})}})(jQuery);
/* ------------------------------------------------------------------------
 * Class: prettyLoader
 * Use: A unified solution for AJAX loader
 * Author: Stephane Caron (http://www.no-margin-for-errors.com)
 * Version: 1.0.1
 * ------------------------------------------------------------------------- */

(function ($) {
	$.prettyLoader = { version: '1.0.1' }; $.prettyLoader = function (settings) {
		settings = jQuery.extend({ animation_speed: 'fast', bind_to_ajax: true, delay: false, loader: '/public/images/prettyLoader/ajax-loader.gif', offset_top: 13, offset_left: 10 }, settings); scrollPos = _getScroll(); imgLoader = new Image(); imgLoader.onerror = function () { alert('Preloader image cannot be loaded. Make sure the path is correct in the settings and that the image is reachable.'); }; imgLoader.src = settings.loader; if (settings.bind_to_ajax)
jQuery(document).ajaxStart(function(){$.prettyLoader.show()}).ajaxStop(function(){$.prettyLoader.hide()});$.prettyLoader.positionLoader=function(e){e=e?e:window.event;cur_x=(e.clientX)?e.clientX:cur_x;cur_y=(e.clientY)?e.clientY:cur_y;left_pos=cur_x+settings.offset_left+scrollPos['scrollLeft'];top_pos=cur_y+settings.offset_top+scrollPos['scrollTop'];$('.prettyLoader').css({'top':top_pos,'left':left_pos});}
$.prettyLoader.show=function(delay){if($('.prettyLoader').size()>0)return;scrollPos=_getScroll();$('<div></div>').addClass('prettyLoader').addClass('prettyLoader_'+settings.theme).appendTo('body').hide();if($.browser.msie&&$.browser.version==6)
$('.prettyLoader').addClass('pl_ie6');$('<img />').attr('src',settings.loader).appendTo('.prettyLoader');$('.prettyLoader').fadeIn(settings.animation_speed);$(document).bind('click',$.prettyLoader.positionLoader);$(document).bind('mousemove',$.prettyLoader.positionLoader);$(window).scroll(function(){scrollPos=_getScroll();$(document).triggerHandler('mousemove');});delay=(delay)?delay:settings.delay;if(delay){setTimeout(function(){$.prettyLoader.hide()},delay);}};$.prettyLoader.hide=function(){$(document).unbind('click',$.prettyLoader.positionLoader);$(document).unbind('mousemove',$.prettyLoader.positionLoader);$(window).unbind('scroll');$('.prettyLoader').fadeOut(settings.animation_speed,function(){$(this).remove();});};function _getScroll(){if(self.pageYOffset){return{scrollTop:self.pageYOffset,scrollLeft:self.pageXOffset};}else if(document.documentElement&&document.documentElement.scrollTop){return{scrollTop:document.documentElement.scrollTop,scrollLeft:document.documentElement.scrollLeft};}else if(document.body){return{scrollTop:document.body.scrollTop,scrollLeft:document.body.scrollLeft};};};return this;};})(jQuery);

/* jQuery Carousel 0.9.5
Copyright 2008-2009 Thomas Lanciaux and Pierre Bertet.
This software is licensed under the CC-GNU LGPL <http://creativecommons.org/licenses/LGPL/2.1/>
*/
(function(c){c.fn.carousel=function(j){var j=c.extend({direction:"horizontal",loop:false,dispItems:1,pagination:false,paginationPosition:"inside",nextBtn:'<a role="button">Next</a>',prevBtn:'<a role="button">Previous</a>',btnsPosition:"inside",nextBtnInsert:"appendTo",prevBtnInsert:"prependTo",nextBtnInsertFn:false,prevBtnInsertFn:false,autoSlide:false,autoSlideInterval:3000,delayAutoSlide:false,combinedClasses:false,effect:"slide",slideEasing:"swing",animSpeed:"normal",equalWidths:"true",verticalMargin:0,callback:function(){},useAddress:false,adressIdentifier:"carousel",tabLabel:function(k){return k},showEmptyItems:true},j);if(j.btnsPosition=="outside"){j.prevBtnInsert="insertBefore";j.nextBtnInsert="insertAfter"}j.delayAutoSlide=j.delayAutoSlide||j.autoSlideInterval;return this.each(function(){var k={$elts:{},params:j,launchOnLoad:[]};k.$elts.carousel=c(this).addClass("js");k.$elts.content=c(this).children().css({position:"absolute",top:0});k.$elts.wrap=k.$elts.content.wrap('<div class="carousel-wrap"></div>').parent().css({overflow:"hidden",position:"relative"});k.steps={first:0,count:k.$elts.content.children().length};k.steps.last=k.steps.count-1;if(c.isFunction(k.params.prevBtnInsertFn)){k.$elts.prevBtn=k.params.prevBtnInsertFn(k.$elts)}else{k.$elts.prevBtn=c(j.prevBtn)[j.prevBtnInsert](k.$elts.carousel)}if(c.isFunction(k.params.nextBtnInsertFn)){k.$elts.nextBtn=k.params.nextBtnInsertFn(k.$elts)}else{k.$elts.nextBtn=c(j.nextBtn)[j.nextBtnInsert](k.$elts.carousel)}k.$elts.nextBtn.addClass("carousel-control next carousel-next");k.$elts.prevBtn.addClass("carousel-control previous carousel-previous");a(k);if(k.params.pagination){i(k)}h(k);c(function(){var n=k.$elts.content.children();var m=0;n.each(function(){$item=c(this);$itemHeight=$item.outerHeight();if($itemHeight>m){m=$itemHeight}});if(k.params.verticalMargin>0){m=m+k.params.verticalMargin}n.height(m);var l=k.$elts.content.children(":first");k.itemWidth=l.outerWidth();if(j.direction=="vertical"){k.contentWidth=k.itemWidth}else{if(j.equalWidths){k.contentWidth=k.itemWidth*k.steps.count}else{k.contentWidth=(function(){var o=0;k.$elts.content.children().each(function(){o+=c(this).outerWidth()});return o})()}}k.$elts.content.width(k.contentWidth);k.itemHeight=m;if(j.direction=="vertical"){k.$elts.content.css({height:k.itemHeight*k.steps.count+"px"});k.$elts.content.parent().css({height:k.itemHeight*k.params.dispItems+"px"})}else{k.$elts.content.parent().css({height:k.itemHeight+"px"})}d(k);c.each(k.launchOnLoad,function(o,p){p()});if(k.params.autoSlide){window.setTimeout(function(){k.autoSlideInterval=window.setInterval(function(){b(k,e(k,"next"))},k.params.autoSlideInterval)},k.params.delayAutoSlide)}})})};function a(j){j.$elts.nextBtn.add(j.$elts.prevBtn).bind("enable",function(){var k=c(this).unbind("click").bind("click",function(){b(j,e(j,(k.is(".next")?"next":"prev")));g(j)}).removeClass("disabled");if(j.params.combinedClasses){k.removeClass("next-disabled previous-disabled")}}).bind("disable",function(){var k=c(this).unbind("click").addClass("disabled");if(j.params.combinedClasses){if(k.is(".next")){k.addClass("next-disabled")}else{if(k.is(".previous")){k.addClass("previous-disabled")}}}}).hover(function(){c(this).toggleClass("hover")})}function i(j){j.$elts.pagination=c('<div class="center-wrap"><div class="carousel-pagination"><p></p></div></div>')[((j.params.paginationPosition=="outside")?"insertAfter":"appendTo")](j.$elts.carousel).find("p");j.$elts.paginationBtns=c([]);j.$elts.content.find("li").each(function(k){if(k%j.params.dispItems==0){j.$elts.paginationBtns=j.$elts.paginationBtns.add(c('<a role="button"><span>'+j.params.tabLabel(j.$elts.paginationBtns.length+1)+"</span></a>").data("firstStep",k))}});j.$elts.paginationBtns.each(function(){c(this).appendTo(j.$elts.pagination)});j.$elts.paginationBtns.slice(0,1).addClass("active");j.launchOnLoad.push(function(){j.$elts.paginationBtns.click(function(k){b(j,c(this).data("firstStep"));g(j)})})}function h(j){if(j.params.useAddress&&c.isFunction(c.fn.address)){c.address.init(function(l){var k=c.address.pathNames();if(k[0]===j.params.adressIdentifier&&!!k[1]){b(j,k[1]-1)}else{c.address.value("/"+j.params.adressIdentifier+"/1")}}).change(function(l){var k=c.address.pathNames();if(k[0]===j.params.adressIdentifier&&!!k[1]){b(j,k[1]-1)}})}else{j.params.useAddress=false}}function b(j,k){j.params.callback(k);f(j,k);j.steps.first=k;d(j);if(j.params.useAddress){c.address.value("/"+j.params.adressIdentifier+"/"+(k+1))}}function e(k,j){if(j=="prev"){if(!k.params.showEmptyItems){if(k.steps.first==0){return((k.params.loop)?(k.steps.count-k.params.dispItems):false)}else{return Math.max(0,k.steps.first-k.params.dispItems)}}else{if((k.steps.first-k.params.dispItems)>=0){return k.steps.first-k.params.dispItems}else{return((k.params.loop)?(k.steps.count-k.params.dispItems):false)}}}else{if(j=="next"){if((k.steps.first+k.params.dispItems)<k.steps.count){if(!k.params.showEmptyItems){return Math.min(k.steps.first+k.params.dispItems,k.steps.count-k.params.dispItems)}else{return k.steps.first+k.params.dispItems}}else{return((k.params.loop)?0:false)}}}}function f(j,k){switch(j.params.effect){case"no":if(j.params.direction=="vertical"){j.$elts.content.css("top",-(j.itemHeight*k)+"px")}else{j.$elts.content.css("left",-(j.itemWidth*k)+"px")}break;case"fade":if(j.params.direction=="vertical"){j.$elts.content.hide().css("top",-(j.itemHeight*k)+"px").fadeIn(j.params.animSpeed)}else{j.$elts.content.hide().css("left",-(j.itemWidth*k)+"px").fadeIn(j.params.animSpeed)}break;default:if(j.params.direction=="vertical"){j.$elts.content.stop().animate({top:-(j.itemHeight*k)+"px"},j.params.animSpeed,j.params.slideEasing)}else{j.$elts.content.stop().animate({left:-(j.itemWidth*k)+"px"},j.params.animSpeed,j.params.slideEasing)}break}}function d(j){if(e(j,"prev")!==false){j.$elts.prevBtn.trigger("enable")}else{j.$elts.prevBtn.trigger("disable")}if(e(j,"next")!==false){j.$elts.nextBtn.trigger("enable")}else{j.$elts.nextBtn.trigger("disable")}if(j.params.pagination){j.$elts.paginationBtns.removeClass("active").filter(function(){return(c(this).data("firstStep")==j.steps.first)}).addClass("active")}}function g(j){if(!!j.autoSlideInterval){window.clearInterval(j.autoSlideInterval)}}})(jQuery);
