using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using static WordleSolver.Constants;

namespace WordleSolver
{
    [JsonConverter(typeof(SolutionJsonConverter))]
    public class Solution
    {
        /// <summary>
        /// The solution to follow for every potential set of clues received.
        /// </summary>
        public IList<Solution> Branches { get; } = new Solution[Clues.ArrayLength];

        /// <summary>
        /// The depth of this guess, starting from 1.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// The next word to guess.
        /// </summary>
        public string Guess { get; private set; }

        private Solution()
        { }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution"/> class.
        /// </summary>
        /// <param name="guess">The word to guess.</param>
        /// <param name="depth">The depth of this guess, starting from 1.</param>
        public Solution(string guess, int depth)
        {
            Guess = guess;
            Depth = depth;
        }

        private class SolutionJsonConverter : JsonConverter<Solution>
        {
            public override bool HandleNull => false;

            public override Solution Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return null;

                var value = new Solution();

                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        return value;

                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException();

                    string propertyName = reader.GetString();
                    if (!reader.Read()) break;

                    if (propertyName.Equals(nameof(value.Guess)))
                        value.Guess = reader.GetString();
                    else if (propertyName.Equals(nameof(value.Depth)))
                        value.Depth = reader.GetInt32();
                    else if (propertyName.Length == WordLength && propertyName.All("012".Contains))
                        value.Branches[Clues.FromString(propertyName).GetHashCode()] = Read(ref reader, typeof(Solution), options);
                    else
                        throw new JsonException($"Unexpected property \"{propertyName}\"");
                }

                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, Solution value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }

                writer.WriteStartObject();
                writer.WriteString(nameof(value.Guess), value.Guess);
                writer.WriteNumber(nameof(value.Depth), value.Depth);
                for (int i = 0; i < Clues.Correct; i++)
                {
                    var propertyValue = value.Branches[i];
                    if (propertyValue == null) continue;
                    var propertyName = Clues.FromHashCode(i).ToString();
                    writer.WritePropertyName(propertyName);
                    Write(writer, propertyValue, options);
                }
                writer.WriteEndObject();
            }
        }
    }
}
