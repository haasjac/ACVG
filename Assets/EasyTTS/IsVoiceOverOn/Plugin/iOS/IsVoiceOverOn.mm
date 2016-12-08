extern "C"
{
    bool _isVoiceOverOn();
}

bool _isVoiceOverOn() {
    return UIAccessibilityIsVoiceOverRunning();
}
