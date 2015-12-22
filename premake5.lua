premake.override(path, "translate", function (base, p)
    return p
end)

function linksthirdparty(name)
    if type(name) == "table" then
        for _, v in ipairs(name) do
            linksthirdparty(v)
        end
        return
    end

    local filename = name .. ".dll"
    local filepath = path.join("ThirdParty", filename)

	links { filepath }
end

solution "ImageArchive"
    configurations { "Debug", "Release" }
    
    framework "4.5"
    links { "System", "System.Core", "System.Web", "System.Data" }

    configuration "Debug"
        defines { "DEBUG", "TRACE" }
        flags { "Symbols" }
        optimize "Off"
        targetdir "bin/Debug"

    configuration "Release"
        defines { "TRACE" }
        optimize "On"
        targetdir "bin/Release"

    project "WebServer"
        kind "SharedLib"
        language "C#"
        location "WebServer"
        files { "WebServer/**.cs" }
        excludes { "WebServer/obj/**.cs" }

    project "TestServer"
        kind "ConsoleApp"
        language "C#"
        location "TestServer"
        files { "TestServer/**.cs" }
        excludes { "TestServer/obj/**.cs" }
        links { "WebServer" }
        linksthirdparty { "Newtonsoft.Json" }
