
require 'json'
require 'yaml'

inputFile = ARGV[0] + '.swagger'
outputFile = ARGV[0] + '.yml'

swagger = open(inputFile, &:read)

data = JSON.parse(swagger)
yml = YAML::dump(data)

File.write(outputFile, yml)

