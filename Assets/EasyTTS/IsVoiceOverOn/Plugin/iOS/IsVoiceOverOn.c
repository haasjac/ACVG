extern "C"
{
    bool _isVoiceOverOn() {
        return UIAccessibilityIsVoiceOverRunning();
    }
}
