/************************************************************************************************
 *  SwiftlyS2 is a scripting framework for Source2-based games.
 *  Copyright (C) 2023-2026 Swiftly Solution SRL via Sava Andrei-Sebastian and it's contributors
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 ************************************************************************************************/

#include "string.h"
#include "plat.h"

#include <api/interfaces/interfaces.h>

#include <algorithm>
#include <cctype>
#include <format>
#include <iterator>
#include <limits>
#include <map>
#include <random>
#include <ranges>
#include <regex>

const std::map<std::string_view, std::string_view> terminalColors = {
    {"[default]", WIN_LINUX("\033[38;2;255;255;255m", "\e[39m")},
    {"[/]", WIN_LINUX("\033[38;2;255;255;255m", "\e[39m")},
    {"[white]", WIN_LINUX("\033[38;2;255;255;255m", "\e[39m")},
    {"[darkred]", WIN_LINUX("\x1B[31m", "\e[31m")},
    {"[lightpurple]", WIN_LINUX("\x1B[95m", "\e[95m")},
    {"[green]", WIN_LINUX("\x1B[32m", "\e[32m")},
    {"[olive]", WIN_LINUX("\x1B[33m", "\e[33m")},
    {"[lime]", WIN_LINUX("\x1B[92m", "\e[92m")},
    {"[red]", WIN_LINUX("\x1B[31m", "\e[31m")},
    {"[gray]", WIN_LINUX("\x1B[37m", "\e[37m")},
    {"[grey]", WIN_LINUX("\x1B[37m", "\e[37m")},
    {"[lightyellow]", WIN_LINUX("\x1B[93m", "\e[93m")},
    {"[yellow]", WIN_LINUX("\x1B[93m", "\e[93m")},
    {"[silver]", WIN_LINUX("\x1B[37m", "\e[37m")},
    {"[bluegrey]", WIN_LINUX("\x1B[94m", "\e[94m")},
    {"[lightblue]", WIN_LINUX("\x1B[94m", "\e[94m")},
    {"[blue]", WIN_LINUX("\x1B[34m", "\e[34m")},
    {"[darkblue]", WIN_LINUX("\x1B[34m", "\e[34m")},
    {"[purple]", WIN_LINUX("\x1B[35m", "\e[35m")},
    {"[magenta]", WIN_LINUX("\x1B[35m", "\e[35m")},
    {"[lightred]", WIN_LINUX("\x1B[91m", "\e[91m")},
    {"[gold]", WIN_LINUX("\x1B[93m", "\e[93m")},
    {"[orange]", WIN_LINUX("\x1B[33m", "\e[33m")},

    {"[bgdefault]", WIN_LINUX("\x1B[40m", "\e[40m")},
    {"[bgdarkred]", WIN_LINUX("\x1B[41m", "\e[41m")},
    {"[bglightpurple]", WIN_LINUX("\x1B[105m", "\e[105m")},
    {"[bggreen]", WIN_LINUX("\x1B[42m", "\e[42m")},
    {"[bgolive]", WIN_LINUX("\x1B[43m", "\e[43m")},
    {"[bglime]", WIN_LINUX("\x1B[102m", "\e[102m")},
    {"[bgred]", WIN_LINUX("\x1B[41m", "\e[41m")},
    {"[bggray]", WIN_LINUX("\x1B[47m", "\e[47m")},
    {"[bggrey]", WIN_LINUX("\x1B[47m", "\e[47m")},
    {"[bglightyellow]", WIN_LINUX("\x1B[103m", "\e[103m")},
    {"[bgyellow]", WIN_LINUX("\x1B[103m", "\e[103m")},
    {"[bgsilver]", WIN_LINUX("\x1B[47m", "\e[47m")},
    {"[bgbluegrey]", WIN_LINUX("\x1B[104m", "\e[104m")},
    {"[bglightblue]", WIN_LINUX("\x1B[104m", "\e[104m")},
    {"[bgblue]", WIN_LINUX("\x1B[44m", "\e[44m")},
    {"[bgdarkblue]", WIN_LINUX("\x1B[44m", "\e[44m")},
    {"[bgpurple]", WIN_LINUX("\x1B[45m", "\e[45m")},
    {"[bgmagenta]", WIN_LINUX("\x1B[45m", "\e[45m")},
    {"[bglightred]", WIN_LINUX("\x1B[101m", "\e[101m")},
    {"[bggold]", WIN_LINUX("\x1B[103m", "\e[103m")},
    {"[bgorange]", WIN_LINUX("\x1B[43m", "\e[43m")},
};

constexpr std::string_view terminalPrefixColors[] = {
    "[default]",
    "[/]",
    "[white]",
    "[darkred]",
    "[lightpurple]",
    "[green]",
    "[olive]",
    "[lime]",
    "[red]",
    "[lightyellow]",
    "[yellow]",
    "[bluegrey]",
    "[lightblue]",
    "[blue]",
    "[darkblue]",
    "[purple]",
    "[magenta]",
    "[lightred]",
    "[gold]",
    "[orange]",
};

const std::map<std::string_view, std::string_view> colors = {
    {"[default]", "\x01"},
    {"[/]", "\x01"},
    {"[white]", "\x01"},
    {"[darkred]", "\x02"},
    {"[lightpurple]", "\x03"},
    {"[green]", "\x04"},
    {"[olive]", "\x05"},
    {"[lime]", "\x06"},
    {"[red]", "\x07"},
    {"[gray]", "\x08"},
    {"[grey]", "\x08"},
    {"[lightyellow]", "\x09"},
    {"[yellow]", "\x09"},
    {"[silver]", "\x0A"},
    {"[bluegrey]", "\x0A"},
    {"[lightblue]", "\x0B"},
    {"[blue]", "\x0B"},
    {"[darkblue]", "\x0C"},
    {"[purple]", "\x0E"},
    {"[magenta]", "\x0E"},
    {"[lightred]", "\x0F"},
    {"[gold]", "\x10"},
    {"[orange]", "\x10"},
};

std::string ProcessColor(std::string str, int team)
{
    const std::string_view teamColor = team == 3 ? "[lightblue]" : (team == 2 ? "[yellow]" : "[lightpurple]");
    str = replace_case_insensitive(str, "[teamcolor]", teamColor);
    for (const auto& [name, code] : colors)
        str = replace_case_insensitive(str, name, code);
    return str;
}

std::string ClearColors(std::string str)
{
    str = replace_case_insensitive(str, "[teamcolor]", "");
    for (const auto name : std::views::keys(colors))
        str = replace_case_insensitive(str, name, "");
    return str;
}

std::string TerminalProcessColor(std::string str)
{
    for (const auto& [name, code] : terminalColors)
        str = replace_case_insensitive(str, name, code);
    return str;
}

std::string ClearTerminalColors(std::string str)
{
    for (const auto name : std::views::keys(terminalColors))
        str = replace_case_insensitive(str, name, "");
    return str;
}

std::string StripAnsiEscapes(std::string_view str)
{
    std::string out;
    out.reserve(str.size());
    for (size_t i = 0; i < str.size(); ++i)
    {
        if (str[i] == '\x1b' && i + 1 < str.size() && str[i + 1] == '[')
        {
            i += 2;
            while (i < str.size() && !(str[i] >= 0x40 && str[i] <= 0x7E))
                ++i;
        }
        else
        {
            out += str[i];
        }
    }
    return out;
}

std::string GetTerminalStringColor(const std::string& plugin_name)
{
    const auto hash = hash_64_fnv1a_const(plugin_name.c_str());
    return std::string(terminalPrefixColors[hash % std::size(terminalPrefixColors)]);
}

std::string replace(std::string str, std::string_view from, std::string_view to)
{
    if (from.empty())
        return str;

    for (size_t pos = 0; (pos = str.find(from, pos)) != std::string::npos; pos += to.size())
        str.replace(pos, from.size(), to);
    return str;
}

std::string replace_case_insensitive(std::string str, std::string_view from, std::string_view to)
{
    if (from.empty())
        return str;

    const std::string from_lower = str_tolower(from);
    std::string str_lower = str_tolower(str);

    for (size_t pos = 0; (pos = str_lower.find(from_lower, pos)) != std::string::npos; pos += to.size())
    {
        str.replace(pos, from.size(), to);
        str_lower.replace(pos, from.size(), to);
    }
    return str;
}

std::vector<std::string> explode(std::string_view str, std::string_view delimiter)
{
    if (str.empty()) return {};
    if (delimiter.empty()) return {std::string(str)};

    std::vector<std::string> res;
    size_t pos_start = 0;
    size_t pos_end;

    while ((pos_end = str.find(delimiter, pos_start)) != std::string_view::npos)
    {
        res.emplace_back(str.substr(pos_start, pos_end - pos_start));
        pos_start = pos_end + delimiter.size();
    }

    res.emplace_back(str.substr(pos_start));
    return res;
}

std::set<std::string> explodeToSet(std::string_view str, std::string_view delimiter)
{
    const auto tokens = explode(str, delimiter);
    return std::set<std::string>(tokens.begin(), tokens.end());
}

std::string implode(const std::vector<std::string>& elements, const std::string& delimiter)
{
    auto joined = elements | std::views::join_with(delimiter);
    return std::string(joined.begin(), joined.end());
}

std::string str_tolower(std::string_view s)
{
    std::string out(s);
    std::ranges::transform(out, out.begin(), [](unsigned char c) { return static_cast<char>(std::tolower(c)); });
    return out;
}

std::string str_toupper(std::string_view s)
{
    std::string out(s);
    std::ranges::transform(out, out.begin(), [](unsigned char c) { return static_cast<char>(std::toupper(c)); });
    return out;
}

int32_t genrand()
{
    static thread_local std::mt19937 rng{std::random_device{}()};
    return std::uniform_int_distribution<int32_t>(0, INT_MAX)(rng);
}

std::string get_uuid()
{
    return std::format(
        "{:04x}{:04x}-{:04x}-{:04x}-{:04x}-{:04x}{:04x}{:04x}",
        (genrand() & 0xFFFF), (genrand() & 0xFFFF),
        (genrand() & 0xFFFF),
        ((genrand() & 0x0fff) | 0x4000),
        (genrand() % 0x3fff + 0x8000),
        (genrand() & 0xFFFF), (genrand() & 0xFFFF), (genrand() & 0xFFFF));
}

std::vector<std::string> TokenizeCommand(std::string_view cmd)
{
    std::vector<std::string> tokens;
    std::string tmp_token;

    bool single_quote = false;
    bool double_quote = false;
    bool token_had_quotes = false;

    const auto byteAt = [&](size_t index) { return static_cast<unsigned char>(cmd[index]); };

    for (size_t index = 0; index < cmd.size(); ++index)
    {
        const unsigned char c = byteAt(index);
        // VALVE USE ZWSP FOR QUOTES IN CHAT AND NO ONE KNOWS FUCKING WHY
        const bool is_replaced_double_quote = c == 0xE2 && index + 2 < cmd.size() && byteAt(index + 1) == 0x80 && byteAt(index + 2) == 0x8B;

        if ((c == '"' || is_replaced_double_quote) && !single_quote) {
            double_quote = !double_quote;
            token_had_quotes = true;

            if (is_replaced_double_quote)
                index += 2;

            continue;
        }
        else if (c == '\'' && !double_quote) {
            single_quote = !single_quote;
            token_had_quotes = true;
            continue;
        }

        if (std::isspace(c) && !single_quote && !double_quote) {
            if (!tmp_token.empty() || token_had_quotes) {
                tokens.push_back(std::move(tmp_token));
                tmp_token.clear();
                token_had_quotes = false;
            }
        }
        else {
            tmp_token += static_cast<char>(c);
        }
    }

    if (!tmp_token.empty() || token_had_quotes)
        tokens.push_back(std::move(tmp_token));

    return tokens;
}

std::string& trim(std::string& s, std::string_view t)
{
    s.erase(s.find_last_not_of(t) + 1);
    s.erase(0, s.find_first_not_of(t));
    return s;
}

std::string RemoveHtmlTags(const std::string& input)
{
    static const std::regex pattern("<(/?)(div|font)[^>]*>");
    return std::regex_replace(input, pattern, "");
}
