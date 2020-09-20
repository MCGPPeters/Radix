FROM gitpod/workspace-dotnet-vnc

# Install custom tools, runtimes, etc.
# For example "bastet", a command-line tetris clone:
# RUN brew install bastet
#
# More information: https://www.gitpod.io/docs/config-docker/

USER gitpod
RUN mkdir $HOME/dotnet_install && cd $HOME/dotnet_install
RUN curl -H 'Cache-Control: no-cache' -L https://aka.ms/install-dotnet-preview -o install-dotnet-preview.sh
RUN sudo bash install-dotnet-preview.sh
