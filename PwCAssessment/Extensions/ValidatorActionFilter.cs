
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PwCAssessment.DTOs;

namespace PwCAssessment.Extensions;

public class ValidatorActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
          
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new GenericResponse<object> { Data = null, Message = string.Join(Environment.NewLine, GetErrorListFromModelState(context.ModelState)) });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //Do nothing
    }

    private static IEnumerable<string> GetErrorListFromModelState
        (ModelStateDictionary modelState)
    {
        var query = from state in modelState.Values
            from error in state.Errors
            select error.ErrorMessage;

        var errorList = query.ToList();
        return errorList;
    }
}

public class JsonNullableStringEnumConverter : JsonConverterFactory
{
    readonly JsonStringEnumConverter _stringEnumConverter;

    public JsonNullableStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
    {
        _stringEnumConverter = new(namingPolicy, allowIntegerValues);
    }

    public override bool CanConvert(Type typeToConvert)
        => Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var type = Nullable.GetUnderlyingType(typeToConvert)!;
        return (JsonConverter?)Activator.CreateInstance(typeof(ValueConverter<>).MakeGenericType(type),
            _stringEnumConverter.CreateConverter(type, options));
    }

    class ValueConverter<T> : JsonConverter<T?>
        where T : struct, Enum
    {
        readonly JsonConverter<T> _converter;

        public ValueConverter(JsonConverter<T> converter)
        {
            this._converter = converter;
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    reader.Read();
                    return null;
                }

                return _converter.Read(ref reader, typeof(T), options);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                _converter.Write(writer, value.Value, options);
        }
    }
}
