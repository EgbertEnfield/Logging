namespace LoggingNcore {
    public enum Level {
        Debug,
        Info,
        Warn,
        Error,
        Except,
        Critical,
        Disabled = 9999, // デフォとしてインスタンス化したクラスを無効化する用
    }
}
