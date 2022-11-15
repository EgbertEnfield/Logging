namespace Logging.NetCore {
    public enum Level {
        Debug,
        Info,
        Warn,
        Error,
        Except,
        Critical,
        Disabled = 512, // デフォとしてインスタンス化したクラスを無効化する用
    }
}
