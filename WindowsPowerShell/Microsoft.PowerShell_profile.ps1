Set-PSReadLineOption -PredictionSource History
Set-PSReadLineOption -PredictionViewStyle ListView
Set-PSReadLineOption -EditMode Windows

Import-Module z


# Load custom theme for Windows Terminal
Import-Module posh-git
# Import-Module oh-my-posh
# Set-Theme LazyAdmin

# Set Default location
# Set-Location c:\dev