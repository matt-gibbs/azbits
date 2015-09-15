
require 'json'
require 'yaml'

inputFile = ARGV[0] + '.yaml'
outputFile = ARGV[0] + '.swagger'

yml = open(inputFile, &:read)

data = YAML::load(yml)
json = JSON.dump(data)
prettyJson = JSON.pretty_generate(data)

File.write(outputFile, prettyJson)

