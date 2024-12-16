open System
open Lunar
open System.CommandLine

let printColored text color enable =
    if enable then
        Console.ForegroundColor <- color
    printf text
    Console.ResetColor()
    
let main argv =
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
    
    rootCommand.SetHandler(
        fun count showYi showJi date showColor->
            // let lunar = Lunar.FromDate(DateTime.Today)
            let lunar = Lunar.FromDate(date)

            if showYi then
                let yi = 
                    List.ofSeq (lunar.DayYi :> seq<string>)
                    |> List.truncate count
                printColored "宜: " ConsoleColor.Green showColor
                yi |> List.iter (fun s -> printf $"{s} ")
                printfn ""

            if showJi then
                let ji =
                    List.ofSeq (lunar.DayJi :> seq<string>)
                    |> List.truncate count
                printColored "忌: " ConsoleColor.Red showColor
                ji |> List.iter (fun s -> printf $"{s} ")
                printfn ""
        , countOpt, yiOpt, jiOpt, dateOpt, colorOpt)

    rootCommand.Invoke (argv : string[])

[<EntryPoint>]
let main' argv =
    main argv |> ignore
    0
