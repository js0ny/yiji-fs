open System
open Lunar
open System.CommandLine

let coloredText text color enable = 
    if enable then
        match color with
                       | "Green" -> 32
                       | "Red" -> 31
                       | _ -> 0
        |> fun code -> $"\x1b[{code}m{text}\x1b[0m"
    else
        text
    
let formatYiJi title items count color enableColor =
    let truncated = items |> Seq.truncate count |> Seq.toList
    let header = coloredText title color enableColor
    let content = truncated |> String.concat " "
    $"{header} {content}"
    
let execute count showYi showJi date showColor =
    let lunar = Lunar.FromDate date
    [
        if showYi then
            formatYiJi "宜：" lunar.DayYi count "Green" showColor
        if showJi then
            formatYiJi "忌：" lunar.DayJi count "Red" showColor
    ] |> String.concat "\n" |> printfn "%s"

let createRootCommand () =
    let rootCommand = Command(
        "yiji",
        description = "获得今天的宜忌信息")
    
    let countOpt = Option<int>(
        "--count",
        description = "显示的宜忌数量",
        getDefaultValue = fun () -> 4)
    countOpt.AddAlias "-n"
    
    let yiOpt = Option<bool>(
        "--yi",
        description = "显示宜",
        getDefaultValue = fun () -> true)
    yiOpt.AddAlias "-y"
    
    let jiOpt = Option<bool>(
        "--ji",
        description = "显示忌",
        getDefaultValue = fun () -> true)
    jiOpt.AddAlias "-j"
    
    let dateOpt = Option<DateTime>(
        "--date",
        description = "指定日期，格式为 yyyy-MM-dd",
        getDefaultValue = fun () -> DateTime.Today)
   
    let colorOpt = Option<bool>(
        "--color",
        description = "是否使用颜色",
        getDefaultValue = fun () -> true)
    colorOpt.AddAlias "-c"
    
    rootCommand.AddOption countOpt
    rootCommand.AddOption yiOpt
    rootCommand.AddOption jiOpt
    rootCommand.AddOption dateOpt
    rootCommand.AddOption colorOpt
    
    rootCommand.SetHandler(execute, countOpt, yiOpt, jiOpt, dateOpt, colorOpt)
    rootCommand

[<EntryPoint>]
let main argv =
    createRootCommand().Invoke(argv) |> ignore
    0
